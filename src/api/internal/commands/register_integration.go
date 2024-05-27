package commands

import (
	"context"

	"github.com/Bergion/social-pilot/internal/constants"
	"github.com/Bergion/social-pilot/internal/models"
	"github.com/Bergion/social-pilot/pkg/mediator"
	"go.mongodb.org/mongo-driver/mongo"
)

type RegisterIntegrationCommand struct {
	UserId     string             `bson:"userId" json:"userId"`
	PlatformId constants.Platform `bson:"platformId" json:"platformId"`
	ApiKey     string             `bson:"apiKey" json:"apiKey"`
}

type RegisterIntegrationCommandHandler struct {
	db *mongo.Database
}

func NewRegisterIntegrationCommandHandler(db *mongo.Database) *RegisterIntegrationCommandHandler {
	return &RegisterIntegrationCommandHandler{db: db}
}

func (h *RegisterIntegrationCommandHandler) Handle(request RegisterIntegrationCommand,
	ctx context.Context) (mediator.Unit, error) {
	// TODO Validation and mapping
	h.db.Collection(models.IntegrationCollectionName).InsertOne(ctx, request)

	return mediator.Unit{}, nil
}
