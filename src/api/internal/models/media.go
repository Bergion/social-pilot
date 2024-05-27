package models

import "go.mongodb.org/mongo-driver/bson/primitive"

type MediaFile struct {
	Id     primitive.ObjectID `bson:"_id,omitempty" json:"id,omitempty"`
	UserId string             `bson:"userId" json:"userId"`
	Url    string             `bson:"url" json:"url"`
}

const MediaFileCollectionName = "media"
