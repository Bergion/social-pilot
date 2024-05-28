package commands

import (
	"context"
	"errors"
	"fmt"
	"log"
	"mime/multipart"
	"os"
	"strings"
	"sync"

	"github.com/Bergion/social-pilot/internal/auth"
	"github.com/Bergion/social-pilot/internal/models"
	"github.com/Bergion/social-pilot/pkg/mediator"
	"github.com/aws/aws-sdk-go-v2/aws"
	"github.com/aws/aws-sdk-go-v2/service/s3"
	"github.com/google/uuid"
	"go.mongodb.org/mongo-driver/mongo"
)

type UploadFileCommand struct {
	Files []*multipart.FileHeader
}

type UploadFileCommandHandler struct {
	db       *mongo.Database
	s3Client *s3.Client
}

type UploadResult struct {
	Filename string
	Key      string
	Error    error
}

func NewUploadFileCommandHandler(db *mongo.Database,
	s3Client *s3.Client) *UploadFileCommandHandler {
	return &UploadFileCommandHandler{db, s3Client}
}

func (h *UploadFileCommandHandler) Handle(request UploadFileCommand,
	ctx context.Context) (mediator.Unit, error) {

	user, ok := auth.GetUserFromContext(ctx)
	if !ok {
		return mediator.Unit{}, errors.New("user is unauthorized")
	}

	var wg sync.WaitGroup

	bucketName := os.Getenv("S3_BUCKET_NAME")
	if bucketName == "" {
		return mediator.Unit{}, errors.New("S3_BUCKET_NAME environment variable is not set")
	}

	resultChannel := make(chan UploadResult, len(request.Files))

	// TODO limit the number of goroutines
	for _, fileHeader := range request.Files {
		wg.Add(1)
		go func(fileHeader *multipart.FileHeader) {
			defer wg.Done()
			file, err := fileHeader.Open()
			if err != nil {
				resultChannel <- UploadResult{
					Filename: fileHeader.Filename,
					Key:      "",
					Error:    err,
				}
				return
			}

			defer file.Close()
			objectKey := uuid.NewString()
			_, err = h.s3Client.PutObject(ctx, &s3.PutObjectInput{
				Bucket: aws.String(bucketName),
				Key:    aws.String(objectKey),
				Body:   file,
			})
			if err != nil {
				resultChannel <- UploadResult{
					Filename: fileHeader.Filename,
					Key:      objectKey,
					Error:    err,
				}
				return
			}

			resultChannel <- UploadResult{
				Filename: fileHeader.Filename,
				Key:      objectKey,
				Error:    err,
			}
		}(fileHeader)
	}

	go func() {
		wg.Wait()
		close(resultChannel)
	}()

	mediaFiles := make([]interface{}, 0, len(request.Files))
	failedFiles := make([]string, 0, len(request.Files))
	for result := range resultChannel {
		if result.Error != nil {
			log.Print(result.Error)
			failedFiles = append(failedFiles, result.Filename)
			continue
		}

		mediaFiles = append(mediaFiles,
			models.MediaFile{
				UserId: user.Id,
				Key:    result.Key,
				Bucket: bucketName,
			})
	}

	collection := h.db.Collection(models.MediaFileCollectionName)
	if _, err := collection.InsertMany(ctx, mediaFiles); err != nil {
		log.Print(err.Error())
		return mediator.Unit{}, errors.New("error saving files")
	}

	if len(failedFiles) != 0 {
		return mediator.Unit{}, errors.New(fmt.Sprint("unable to upload files: ", strings.Join(failedFiles, ", ")))
	}

	return mediator.Unit{}, nil
}
