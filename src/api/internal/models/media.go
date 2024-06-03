package models

type Media struct {
	Key    string `bson:"key" json:"key"`
	Bucket string `bson:"bucket" json:"bucket"`
}
