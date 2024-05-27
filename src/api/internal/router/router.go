package router

import (
	"net/http"

	"github.com/Bergion/social-pilot/internal/controllers"
	"github.com/go-chi/chi"
	"github.com/go-chi/chi/v5/middleware"
)

func NewRouter() http.Handler {
	r := chi.NewRouter()

	r.Use(middleware.Logger)
	r.Use(middleware.Heartbeat("/health"))

	integrationController := controllers.NewIntegrationController()

	r.Post("/integrations/connect", integrationController.RegisterIntegration)
	r.Put("/integrations/{id}", integrationController.UpdateIntegration)

	mediaController := controllers.NewMediaController()

	r.Post("/media/upload", mediaController.UploadFile)
	return r
}
