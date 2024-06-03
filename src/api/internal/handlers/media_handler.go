package handlers

import (
	"encoding/json"
	"net/http"

	"github.com/Bergion/social-pilot/internal/storage"
)

type MediaHandler struct {
	storage storage.FileStorage
}

func NewMediaHandler(storage storage.FileStorage) *MediaHandler {
	return &MediaHandler{storage}
}

func (h *MediaHandler) GetPresignedUrl(w http.ResponseWriter, r *http.Request) {
	var filenames []string
	w.Header().Set("Content-Type", "application/json")
	if err := json.NewDecoder(r.Body).Decode(&filenames); err != nil {
		w.WriteHeader(http.StatusBadRequest)
		return
	}

	presignedUrls, err := h.storage.GeneratePresignedUrls(r.Context(), filenames)
	if err != nil {
		w.WriteHeader(http.StatusInternalServerError)
		return
	}

	response, _ := json.Marshal(presignedUrls)

	w.Write(response)
}
