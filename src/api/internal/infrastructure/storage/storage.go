package storage

import (
	"context"
	"fmt"
	"log"
	"os"
	"time"

	"github.com/Bergion/social-pilot/internal/config"
	"github.com/aws/aws-sdk-go-v2/aws"
	"github.com/aws/aws-sdk-go-v2/service/s3"
	"github.com/google/uuid"
	"golang.org/x/sync/errgroup"
)

type PresignedURL struct {
	URL      string
	Filename string
}

type FileStorage interface {
	GeneratePresignedUrls(context.Context, []string) ([]PresignedURL, error)
}

type s3FileStorage struct {
	s3Client *s3.Client
}

func NewAWSFileStorage() FileStorage {
	s3Client := s3.NewFromConfig(config.LoadAWSConfig())
	return &s3FileStorage{s3Client}
}

func (s *s3FileStorage) GeneratePresignedUrls(ctx context.Context,
	filenames []string) ([]PresignedURL, error) {

	// TODO Set limit
	eg, egCtx := errgroup.WithContext(ctx)

	presignedClient := s3.NewPresignClient(s.s3Client)
	bucketName := os.Getenv("S3_BUCKET_NAME")

	presignedUrls := make([]PresignedURL, len(filenames))
	for i, filename := range filenames {
		i, filename := i, filename
		eg.Go(func() error {
			key := fmt.Sprint(uuid.New(), filename)
			req, err := presignedClient.PresignPutObject(egCtx,
				&s3.PutObjectInput{
					Bucket: aws.String(bucketName),
					Key:    aws.String(key),
				},
				s3.WithPresignExpires(time.Minute*15),
			)

			if err == nil {
				presignedUrls[i] = PresignedURL{req.URL, filename}
			}

			return err
		})
	}

	if err := eg.Wait(); err != nil {
		log.Print(err)
		return nil, err
	}

	return presignedUrls, nil
}
