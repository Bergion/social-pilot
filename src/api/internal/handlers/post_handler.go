package handlers

import (
	"encoding/json"
	"log"
	"net/http"

	"github.com/Bergion/social-pilot/internal/auth"
	"github.com/Bergion/social-pilot/internal/models"
	"go.mongodb.org/mongo-driver/mongo"
)

type PostHandler struct {
	db *mongo.Database
}

func NewPostHandler(db *mongo.Database) *PostHandler {
	return &PostHandler{db}
}

func (h *PostHandler) CreatePost(w http.ResponseWriter, r *http.Request) {
	ctx := r.Context()
	w.Header().Set("Content-Type", "application/json")

	var post models.Post
	if err := json.NewDecoder(r.Body).Decode(&post); err != nil {
		w.WriteHeader(http.StatusBadRequest)
		return
	}

	user, _ := auth.UserFromContext(ctx)
	post.UserId = user.Id

	collection := h.db.Collection(models.PostCollectionName)

	res, err := collection.InsertOne(ctx, post)
	if err != nil {
		log.Println(err)
		return
	}

	// Publish post to SQS if Status == InstantPublish

	response, _ := json.Marshal(struct {
		Success bool        `json:"success"`
		PostId  interface{} `json:"postId"`
	}{
		Success: true,
		PostId:  res.InsertedID,
	})

	w.Write(response)
}
