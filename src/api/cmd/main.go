package main

import (
	"net/http"

	"github.com/Bergion/social-pilot/internal/commands"
	"github.com/Bergion/social-pilot/internal/config"
	"github.com/Bergion/social-pilot/internal/db"
	"github.com/Bergion/social-pilot/internal/handlers"
	"github.com/Bergion/social-pilot/internal/router"
	"github.com/Bergion/social-pilot/internal/storage"
	"github.com/Bergion/social-pilot/pkg/mediator"
	"github.com/Bergion/social-pilot/pkg/publisher"
	"github.com/Bergion/social-pilot/server"
	"go.mongodb.org/mongo-driver/mongo"
	"go.uber.org/fx"
)

func main() {
	fx.New(
		fx.Provide(
			server.NewHTTPServer,
			router.NewRouter,
			handlers.NewMediaHandler,
			handlers.NewPostHandler,
			db.NewMongoClient,
			config.LoadAWSConfig,
			fx.Annotate(
				storage.NewAWSFileStorage,
				fx.As(new(storage.FileStorage)),
			),
			fx.Annotate(
				publisher.NewSNSPublisher,
				fx.As(new(publisher.Publisher)),
			),
		),
		fx.Invoke(
			func(*http.Server) {},
			config.LoadConfig,
			registerHandlers,
		),
	).Run()
}

func registerHandlers(client *mongo.Client, publisher publisher.Publisher) {
	db := client.Database("db")
	mediator.RegisterHandler(commands.NewCreatePostHandler(db, publisher))
}
