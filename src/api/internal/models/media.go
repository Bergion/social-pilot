package models

import "go.mongodb.org/mongo-driver/bson/primitive"

type MediaFile struct {
	Id     primitive.ObjectID `bson:"_id,omitempty" json:"id,omitempty"`
	UserId string             `bson:"userId" json:"userId"`
	Key    string             `bson:"key" json:"key"`
	Bucket string             `bson:"bucket" json:"bucket"`
}

const MediaFileCollectionName = "media"
