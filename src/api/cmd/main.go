package main

import (
	"fmt"
	"log"
	"net/http"
	"os"

	"github.com/Bergion/social-pilot/internal/commands"
	"github.com/Bergion/social-pilot/internal/config"
	"github.com/Bergion/social-pilot/internal/db"
	"github.com/Bergion/social-pilot/internal/router"
	"github.com/Bergion/social-pilot/pkg/mediator"
	"github.com/aws/aws-sdk-go-v2/service/s3"
)

func main() {
	config.LoadConfig()

	mongoClient, err := db.NewMongoClient(os.Getenv("CONNECTION_STRING"))
	if err != nil {
		log.Fatal(err)
	}
	defer mongoClient.Close()

	db := mongoClient.Client.Database("db")
	router := router.NewRouter()

	s3Client := s3.NewFromConfig(config.LoadAWSConfig())

	mediator.RegisterHandler(commands.NewRegisterIntegrationCommandHandler(db))
	mediator.RegisterHandler(commands.NewUploadFileCommandHandler(db, s3Client))

	fmt.Println("Server started on :8080")
	if err = http.ListenAndServe(":8080", router); err != nil {
		log.Fatal(err)
	}
}
