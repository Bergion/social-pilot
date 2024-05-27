package controllers

import (
	"net/http"

	"github.com/Bergion/social-pilot/internal/commands"
	"github.com/Bergion/social-pilot/pkg/mediator"
)

type MediaController struct {
}

func NewMediaController() *MediaController {
	return &MediaController{}
}

func (c *MediaController) UploadFile(w http.ResponseWriter, r *http.Request) {
	r.ParseMultipartForm(10 << 20)
	files, ok := r.MultipartForm.File["media"]
	if !ok {
		http.Error(w, "unknown file upload error", http.StatusBadRequest)
		return
	}

	_, err := mediator.Send[commands.UploadFileCommand, mediator.Unit](
		commands.UploadFileCommand{Files: files}, r.Context())
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusOK)
}
