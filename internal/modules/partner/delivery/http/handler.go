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

// Create @Summary Create Partner
// @Tags Partners
// @Param body body service.CreatePartnerRequest true "Partner Info"
// @Success 201 {object} api.BaseResponse
// @Router /partners [post]
func (h *PartnerHandler) Create(w http.ResponseWriter, r *http.Request) {
	var req service.CreatePartnerRequest
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid JSON format"))
		return
	}
	if err := h.service.Create(r.Context(), req); err != nil {
		h.respondWithError(w, err)
		return
	}
	api.WriteJSON(w, http.StatusCreated, api.SuccessResponse("Partner created successfully", nil))
}

// List @Summary List Partners
// @Tags Partners
// @Success 200 {array} service.PartnerResponse
// @Router /partners [get]
func (h *PartnerHandler) List(w http.ResponseWriter, r *http.Request) {
	partners, err := h.service.List(r.Context())
	if err != nil {
		h.respondWithError(w, err)
		return
	}
	api.WriteJSON(w, http.StatusOK, api.SuccessResponse("Partners retrieved successfully", partners))
}

// Update @Summary Update Partner
// @Tags Partners
// @Param id path int true "Partner ID"
// @Param body body service.UpdatePartnerRequest true "Update Info"
// @Success 200 {object} api.BaseResponse
// @Router /partners/{id} [put]
func (h *PartnerHandler) Update(w http.ResponseWriter, r *http.Request) {
	id, err := h.parseID(r)
	if err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid ID format"))
		return
	}

	var req service.UpdatePartnerRequest
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid JSON format"))
		return
	}

	if err := h.service.Update(r.Context(), id, req); err != nil {
		h.respondWithError(w, err)
		return
	}
	api.WriteJSON(w, http.StatusOK, api.SuccessResponse("Partner updated successfully", nil))
}

// Delete @Summary Delete Partner
// @Tags Partners
// @Param id path int true "Partner ID"
// @Success 200 {object} api.BaseResponse
// @Router /partners/{id} [delete]
func (h *PartnerHandler) Delete(w http.ResponseWriter, r *http.Request) {
	id, err := h.parseID(r)
	if err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid ID format"))
		return
	}

	if err := h.service.Delete(r.Context(), id); err != nil {
		h.respondWithError(w, err)
		return
	}
	api.WriteJSON(w, http.StatusOK, api.SuccessResponse("Partner deleted successfully", nil))
}

func (h *PartnerHandler) parseID(r *http.Request) (uint, error) {
	idStr := chi.URLParam(r, "id")
	id, err := strconv.ParseUint(idStr, 10, 32)
	if err != nil {
		return 0, err
	}
	return uint(id), nil
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
