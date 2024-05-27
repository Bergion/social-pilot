package models

import (
	"github.com/Bergion/social-pilot/internal/constants"
	"go.mongodb.org/mongo-driver/bson/primitive"
)

type Integration struct {
	Id         primitive.ObjectID `bson:"_id,omitempty" json:"id,omitempty"`
	UserId     string             `bson:"userId" json:"userId"`
	PlatformId constants.Platform `bson:"platformId" json:"platformId"`
	ApiKey     string             `bson:"apiKey" json:"apiKey"`
}

const IntegrationCollectionName = "integration"
