package commands

import (
	"context"

	"github.com/Bergion/social-pilot/internal/models"
)

type CreatePostCommand struct {
	Text        string
	Media       []models.Media
	Status      int
	PublishDate string
}

type CreatePostCommandHandler struct {
}

func (h CreatePostCommandHandler) Handle(request CreatePostCommand, ctx context.Context) {
	// Persist and publish
}
