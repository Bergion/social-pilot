package commands

import (
	"context"

	"go.mongodb.org/mongo-driver/mongo"
)

type PublishPostCommand struct {
	text string
}

type PublishPostCommandHandler struct {
	db *mongo.Database
}

func NewPublishPostCommandHandler(db *mongo.Database) *PublishPostCommandHandler {
	return &PublishPostCommandHandler{db}
}

func (h *PublishPostCommandHandler) Handle(request PublishPostCommand, ctx context.Context) {
	// user, ok := auth.GetUserFromContext(ctx)
}
