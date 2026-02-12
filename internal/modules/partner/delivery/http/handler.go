package http

import (
	"encoding/json"
	"errors"
	"log"
	"net/http"
	"strconv"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/partner/service"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/api"
	"github.com/go-chi/chi/v5"
)

type PartnerHandler struct {
	service service.PartnerService
}

func NewPartnerHandler(s service.PartnerService) *PartnerHandler {
	return &PartnerHandler{service: s}
}

func (h *PartnerHandler) respondWithError(w http.ResponseWriter, err error) {
	var appErr *api.AppError
	if errors.As(err, &appErr) {
		if appErr.RawErr != nil {
			log.Printf("[System Error]: %v", appErr.RawErr)
		}
		api.WriteJSON(w, appErr.StatusCode, api.ErrorResponse(appErr.ErrMsg))
		return
	}

	log.Printf("[Unknown Error]: %v", err)
	api.WriteJSON(w, http.StatusInternalServerError, api.ErrorResponse("An unexpected error occurred"))
}

func (h *PartnerHandler) Create(w http.ResponseWriter, r *http.Request) {
	var req service.CreatePartnerRequest

	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid JSON format"))
		return
	}
	err := h.service.Create(r.Context(), req)

	if err != nil {
		h.respondWithError(w, err)
		return
	}

	api.WriteJSON(w, http.StatusCreated, api.SuccessResponse("Partner created successfully", nil))
}

func (h *PartnerHandler) List(w http.ResponseWriter, r *http.Request) {
	partners, err := h.service.List(r.Context())
	if err != nil {
		h.respondWithError(w, err)
		return
	}

	api.WriteJSON(w, http.StatusOK, api.SuccessResponse("Partners retrieved successfully", partners))
}

func (h *PartnerHandler) Update(w http.ResponseWriter, r *http.Request) {
	idStr := chi.URLParam(r, "id")
	id, err := strconv.ParseUint(idStr, 10, 32)
	if err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid ID format"))
		return
	}

	var req service.UpdatePartnerRequest
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid JSON format"))
		return
	}

	if err := h.service.Update(r.Context(), uint(id), req); err != nil {
		h.respondWithError(w, err)
		return
	}

	api.WriteJSON(w, http.StatusOK, api.SuccessResponse("Partner updated successfully", nil))
}

func (h *PartnerHandler) Delete(w http.ResponseWriter, r *http.Request) {
	idStr := chi.URLParam(r, "id")
	id, err := strconv.ParseUint(idStr, 10, 32)
	if err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid ID format"))
		return
	}

	if err := h.service.Delete(r.Context(), uint(id)); err != nil {
		h.respondWithError(w, err)
		return
	}

	api.WriteJSON(w, http.StatusOK, api.SuccessResponse("Partner deleted successfully", nil))
}
