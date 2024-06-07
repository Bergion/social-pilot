package handlers

import (
	"encoding/json"
	"net/http"

	"github.com/Bergion/social-pilot/internal/commands"
	"github.com/Bergion/social-pilot/pkg/mediator"
)

type PostHandler struct {
}

func NewPostHandler() *PostHandler {
	return &PostHandler{}
}

func (h *PostHandler) CreatePost(w http.ResponseWriter, r *http.Request) {
	ctx := r.Context()
	w.Header().Set("Content-Type", "application/json")

	var createPostCommand commands.CreatePost
	if err := json.NewDecoder(r.Body).Decode(&createPostCommand); err != nil {
		w.WriteHeader(http.StatusBadRequest)
		return
	}
	res, err := mediator.Send[commands.CreatePost, interface{}](createPostCommand, ctx)

	if err != nil {
		w.WriteHeader(http.StatusInternalServerError)
		return
	}

	response, _ := json.Marshal(struct {
		Success bool        `json:"success"`
		PostId  interface{} `json:"postId"`
	}{
		Success: true,
		PostId:  res,
	})

	w.Write(response)
}
