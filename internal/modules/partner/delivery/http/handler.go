package http

import (
	"encoding/json"
	"net/http"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/partner/service"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/api"
)

type PartnerHandler struct {
	service service.PartnerService
}

func NewPartnerHandler(s service.PartnerService) *PartnerHandler {
	return &PartnerHandler{service: s}
}

func (h *PartnerHandler) Create(w http.ResponseWriter, r *http.Request) {
	var req service.CreatePartnerRequest

	err := json.NewDecoder(r.Body).Decode(&req)
	if err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Incorrect Format Json: "+err.Error()))
		return
	}

	err = h.service.Create(r.Context(), req)
	if err != nil {
		api.WriteJSON(w, http.StatusInternalServerError, api.ErrorResponse(err.Error()))
		return
	}

	response := api.SuccessResponse("Partner created successfully", nil)
	api.WriteJSON(w, http.StatusCreated, response)
}
