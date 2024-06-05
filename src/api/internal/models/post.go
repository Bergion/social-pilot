package models

import "go.mongodb.org/mongo-driver/bson/primitive"

const PostCollectionName = "post"

type Post struct {
	Id           primitive.ObjectID `bson:"id" json:"id"`
	Text         string             `bson:"text" json:"text"`
	Media        []Media            `bson:"media" json:"media"`
	UserId       string             `bson:"userId" json:"userId"`
	Status       int                `bson:"status" json:"status"`
	ScheduledFor string             `bson:"scheduledFor" json:"scheduledFor"`
}
