package router

import (
	"net/http"

	"github.com/Bergion/social-pilot/internal/handlers"
	"github.com/Bergion/social-pilot/internal/storage"
	"github.com/go-chi/chi"
	"github.com/go-chi/chi/v5/middleware"
	"go.mongodb.org/mongo-driver/mongo"
)

func NewRouter(db *mongo.Database) http.Handler {
	r := chi.NewRouter()

	r.Use(middleware.Logger)
	r.Use(middleware.Heartbeat("/health"))

	storage := storage.NewAWSFileStorage()

	mediaHandler := handlers.NewMediaHandler(storage)
	postHandler := handlers.NewPostHandler(db)

	r.Post("/media/upload", mediaHandler.GetPresignedUrl)
	r.Post("/post", postHandler.CreatePost)
	return r
}
