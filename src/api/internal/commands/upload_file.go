package commands

import (
	"context"
	"fmt"
	"mime/multipart"

	"github.com/Bergion/social-pilot/pkg/mediator"
	"github.com/aws/aws-sdk-go-v2/aws"
	"github.com/aws/aws-sdk-go-v2/service/s3"
	"go.mongodb.org/mongo-driver/mongo"
)

type UploadFileCommand struct {
	Files []*multipart.FileHeader
}

type UploadFileCommandHandler struct {
	db       *mongo.Database
	s3Client *s3.Client
}

func NewUploadFileCommandHandler(db *mongo.Database,
	s3Client *s3.Client) *UploadFileCommandHandler {
	return &UploadFileCommandHandler{db, s3Client}
}

func (h *UploadFileCommandHandler) Handle(request UploadFileCommand,
	ctx context.Context) (mediator.Unit, error) {

	// user, ok := auth.GetUserFromContext(ctx)
	// if !ok {
	// 	return mediator.Unit{}, errors.New("user is unauthorized")
	// }

	for _, fileHeader := range request.Files {
		file, err := fileHeader.Open()
		if err != nil {
			return mediator.Unit{}, err
		}

		defer file.Close()

		bucketName := "social-pilot-media-2024"
		objectKey := "objectKey"
		res, err := h.s3Client.PutObject(ctx, &s3.PutObjectInput{
			Bucket: aws.String(bucketName),
			Key:    aws.String(objectKey),
			Body:   file,
		})
		if err != nil {
			return mediator.Unit{}, err
		}

		fmt.Print(res.ResultMetadata)
	}

	return mediator.Unit{}, nil
}
