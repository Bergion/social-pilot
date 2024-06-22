package config

import (
	"context"
	"log"

	"github.com/aws/aws-sdk-go-v2/aws"
	"github.com/aws/aws-sdk-go-v2/config"
	"github.com/joho/godotenv"
)

func LoadConfig() {
	if err := godotenv.Load(); err != nil {
		log.Println("No .env file found, using default environment variables")
	}

	log.Println("Configuration loaded")
}

func LoadAWSConfig() aws.Config {
	cfg, err := config.LoadDefaultConfig(
		context.TODO(),
	)

	if err != nil {
		log.Fatal(err)
	}
	return cfg
}
