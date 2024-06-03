package models

import "go.mongodb.org/mongo-driver/bson/primitive"

type Post struct {
	Id     primitive.ObjectID
	Text   string
	Media  []Media
	UserId string
}
