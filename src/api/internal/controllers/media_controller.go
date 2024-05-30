package controllers

import (
	"encoding/json"
	"net/http"

	"github.com/Bergion/social-pilot/internal/infrastructure/storage"
)

type MediaController struct {
	storage storage.FileStorage
}

func NewMediaController(storage storage.FileStorage) *MediaController {
	return &MediaController{storage}
}

func (c *MediaController) GetPresignedUrl(w http.ResponseWriter, r *http.Request) {
	var filenames []string
	if err := json.NewDecoder(r.Body).Decode(&filenames); err != nil {
		w.WriteHeader(http.StatusBadRequest)
		return
	}

	presignedUrls, err := c.storage.GeneratePresignedUrls(r.Context(), filenames)
	if err != nil {
		w.WriteHeader(http.StatusInternalServerError)
		return
	}

	response, _ := json.Marshal(presignedUrls)

	w.Write(response)
}
