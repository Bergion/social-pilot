package models

type Platform struct {
	Id       int    `bson:"id" json:"id"`
	Name     string `bson:"name" json:"name"`
	PostType string `bson:"postType" json:"postType"`
}
