package router

import (
	"net/http"

	"github.com/Bergion/social-pilot/internal/handlers"
	"github.com/go-chi/chi"
	"github.com/go-chi/chi/v5/middleware"
	"github.com/rs/cors"
)

// func NewRouter(storage storage.FileStorage) http.Handler {
// 	r := chi.NewRouter()

// 	r.Use(middleware.Logger)
// 	r.Use(middleware.Heartbeat("/health"))

// 	mediaHandler := handlers.NewMediaHandler(storage)
// 	postHandler := handlers.NewPostHandler()

// 	r.Post("/media/upload", mediaHandler.GetPresignedUrl)
// 	r.Post("/post", postHandler.CreatePost)

// 	return cors.AllowAll().Handler(r)
// }

func NewRouter(mediaHandler *handlers.MediaHandler,
	postHandler *handlers.PostHandler) http.Handler {
	r := chi.NewRouter()

	r.Use(middleware.Logger)
	r.Use(middleware.Heartbeat("/health"))

	r.Post("/media/upload", mediaHandler.GetPresignedUrl)
	r.Post("/post", postHandler.CreatePost)

	return cors.AllowAll().Handler(r)
}
