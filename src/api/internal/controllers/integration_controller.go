package controllers

import (
	"encoding/json"
	"net/http"

	"github.com/Bergion/social-pilot/internal/commands"
	"github.com/Bergion/social-pilot/pkg/mediator"
)

type IntegrationController struct {
}

func NewIntegrationController() *IntegrationController {
	return &IntegrationController{}
}

func (c *IntegrationController) RegisterIntegration(w http.ResponseWriter, r *http.Request) {
	var command commands.RegisterIntegrationCommand
	if err := json.NewDecoder(r.Body).Decode(&command); err != nil {
		http.Error(w, err.Error(), http.StatusBadRequest)
		return
	}

	_, err := mediator.Send[commands.RegisterIntegrationCommand, mediator.Unit](command, r.Context())
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusCreated)
}

func (c *IntegrationController) UpdateIntegration(w http.ResponseWriter, r *http.Request) {

}
