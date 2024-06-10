package models

const PostCollectionName = "post"

type Post struct {
	Id           interface{} `bson:"id" json:"id"`
	Text         string      `bson:"text" json:"text"`
	Media        []Media     `bson:"media" json:"media"`
	UserId       string      `bson:"userId" json:"userId"`
	Status       int         `bson:"status" json:"status"`
	ScheduledFor string      `bson:"scheduledFor" json:"scheduledFor"`
	Platforms    []Platform  `bson:"platforms" json:"platforms"`
}
