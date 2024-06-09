package commands

import (
	"context"
	"log"

	"github.com/Bergion/social-pilot/internal/auth"
	"github.com/Bergion/social-pilot/internal/models"
	"github.com/Bergion/social-pilot/pkg/publisher"
	"go.mongodb.org/mongo-driver/mongo"
)

type CreatePost struct {
	Text         string            `json:"text"`
	Media        []models.Media    `json:"media"`
	UserId       string            `json:"userId"`
	Status       int               `json:"status"`
	ScheduledFor string            `json:"scheduledFor"`
	Platforms    []models.Platform `json:"platforms"`
}

type CreatePostHandler struct {
	db        *mongo.Database
	publisher publisher.Publisher
}

func (h *CreatePostHandler) Handle(request CreatePost, ctx context.Context) (interface{}, error) {
	user, _ := auth.UserFromContext(ctx)

	post := &models.Post{
		Text:         request.Text,
		Media:        request.Media,
		Status:       request.Status,
		ScheduledFor: request.ScheduledFor,
		Platforms:    request.Platforms,
		UserId:       user.Id,
	}

	collection := h.db.Collection(models.PostCollectionName)

	res, err := collection.InsertOne(ctx, post)
	if err != nil {
		log.Println(err)
		return nil, err
	}

	// remove magic number
	if post.Status == 2 {
		// h.publisher.Publish(ctx)
	}

	return res.InsertedID, nil
}

func NewCreatePostHandler(db *mongo.Database, publisher publisher.Publisher) *CreatePostHandler {
	return &CreatePostHandler{db, publisher}
}
