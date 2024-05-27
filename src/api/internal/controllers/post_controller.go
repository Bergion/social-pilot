package controllers

import "net/http"

type PostController struct {
}

func NewPostController() *PostController {
	return &PostController{}
}

func (c *PostController) PublishPost(w http.ResponseWriter, r *http.Request) {
}

func (c *PostController) UploadFile(w http.ResponseWriter, r *http.Request) {

}
