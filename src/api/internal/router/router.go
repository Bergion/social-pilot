package router

import (
	"net/http"

	"github.com/Bergion/social-pilot/internal/controllers"
	"github.com/Bergion/social-pilot/internal/infrastructure/storage"
	"github.com/go-chi/chi"
	"github.com/go-chi/chi/v5/middleware"
)

func NewRouter() http.Handler {
	r := chi.NewRouter()

	r.Use(middleware.Logger)
	r.Use(middleware.Heartbeat("/health"))

	storage := storage.NewAWSFileStorage()
	mediaController := controllers.NewMediaController(storage)

	r.Post("/media/upload", mediaController.GetPresignedUrl)
	return r
}
