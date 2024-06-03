package main

import (
	"fmt"
	"log"
	"net/http"
	"os"

	"github.com/Bergion/social-pilot/internal/config"
	"github.com/Bergion/social-pilot/internal/db"
	"github.com/Bergion/social-pilot/internal/router"
)

func main() {
	config.LoadConfig()

	mongoClient, err := db.NewMongoClient(os.Getenv("CONNECTION_STRING"))
	if err != nil {
		log.Fatal(err)
	}
	defer mongoClient.Close()

	db := mongoClient.Client.Database("db")

	router := router.NewRouter(db)

	fmt.Println("Server started on :8080")
	if err := http.ListenAndServe(":8080", router); err != nil {
		log.Fatal(err)
	}
}
