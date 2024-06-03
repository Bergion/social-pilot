package storage

import (
	"context"
	"fmt"
	"log"
	"os"
	"time"

	"github.com/Bergion/social-pilot/internal/auth"
	"github.com/Bergion/social-pilot/internal/config"
	"github.com/aws/aws-sdk-go-v2/aws"
	"github.com/aws/aws-sdk-go-v2/service/s3"
	"github.com/google/uuid"
	"golang.org/x/sync/errgroup"
)

type PresignedURL struct {
	URL string
	Key string
}

type FileStorage interface {
	GeneratePresignedUrls(context.Context, []string) (map[string]PresignedURL, error)
}

type s3FileStorage struct {
	s3Client *s3.Client
}

type result struct {
	Filename     string
	PresignedURL PresignedURL
}

func NewAWSFileStorage() FileStorage {
	s3Client := s3.NewFromConfig(config.LoadAWSConfig())
	return &s3FileStorage{s3Client}
}

func (s *s3FileStorage) GeneratePresignedUrls(ctx context.Context,
	filenames []string) (map[string]PresignedURL, error) {

	bucketName := os.Getenv("S3_BUCKET_NAME")
	// TODO Set limit
	eg, egCtx := errgroup.WithContext(ctx)

	user, _ := auth.UserFromContext(ctx)
	presignedClient := s3.NewPresignClient(s.s3Client)

	resultChannel := make(chan result)
	for _, filename := range filenames {
		filename := filename
		eg.Go(func() error {
			key := fmt.Sprintf("%s/%s_%s", user.Id, uuid.New(), filename)
			req, err := presignedClient.PresignPutObject(egCtx,
				&s3.PutObjectInput{
					Bucket: aws.String(bucketName),
					Key:    aws.String(key),
				},
				s3.WithPresignExpires(time.Minute*15),
			)

			if err == nil {
				resultChannel <- result{filename, PresignedURL{req.URL, key}}
			}

			return err
		})
	}

	var err error
	go func() {
		err = eg.Wait()
		close(resultChannel)
	}()

	presignedUrls := make(map[string]PresignedURL)
	for result := range resultChannel {
		presignedUrls[result.Filename] = result.PresignedURL
	}

	if err != nil {
		log.Println(err)
		return nil, err
	}

	return presignedUrls, nil
}
