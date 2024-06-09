package publisher

import (
	"context"
	"log"

	"github.com/aws/aws-sdk-go-v2/aws"
	"github.com/aws/aws-sdk-go-v2/service/sns"
)

type Publisher interface {
	Publish(ctx context.Context, topicArn string, message string) error
}

type SNSPublisher struct {
	client *sns.Client
}

func NewSNSPublisher(cfg aws.Config) *SNSPublisher {
	snsClient := sns.NewFromConfig(cfg)
	return &SNSPublisher{client: snsClient}
}

func (p *SNSPublisher) Publish(ctx context.Context, topic string, message string) error {
	publishInput := &sns.PublishInput{
		TopicArn: aws.String(topic),
		Message:  aws.String(message),
	}

	_, err := p.client.Publish(ctx, publishInput)
	if err != nil {
		log.Println(err.Error())
	}

	return err
}
