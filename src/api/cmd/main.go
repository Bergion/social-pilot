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
	"github.com/Bergion/social-pilot/internal/storage"
	"github.com/Bergion/social-pilot/pkg/mediator"
	"github.com/Bergion/social-pilot/pkg/publisher"
	"github.com/aws/aws-sdk-go-v2/aws"
)

func main() {
	config.LoadConfig()

	mongoClient, err := db.NewMongoClient(os.Getenv("CONNECTION_STRING"))
	if err != nil {
		log.Fatal(err)
	}
	defer mongoClient.Close()

	awsCfg := config.LoadAWSConfig()
	storage := storage.NewAWSFileStorage(awsCfg)

	registerHandlers(mongoClient, awsCfg)

	router := router.NewRouter(storage)

	fmt.Println("Server started on :8080")
	if err := http.ListenAndServe(":8080", router); err != nil {
		log.Fatal(err)
	}
}

func registerHandlers(mongoClient *db.MongoClient, cfg aws.Config) {
	publisher := publisher.NewSNSPublisher(cfg)

	db := mongoClient.Client.Database("db")

	mediator.RegisterHandler(commands.NewCreatePostHandler(db, publisher))
}
