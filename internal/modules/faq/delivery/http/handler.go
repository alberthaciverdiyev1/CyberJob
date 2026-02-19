package http

import (
	"encoding/json"
	"errors"
	"log"
	"net/http"
	"strconv"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/faq/service"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/api"
	"github.com/go-chi/chi/v5"
)

type FaqHandler struct {
	service service.FAQService
}

func NewFaqHandler(s service.FAQService) *FaqHandler {
	return &FaqHandler{service: s}
}

// GetAll @Summary List all FAQs
// @Description Get all frequently asked questions with translations
// @Tags FAQs
// @Produce json
// @Success 200 {array} service.FAQResponse
// @Router /faqs [get]
func (h *FaqHandler) GetAll(w http.ResponseWriter, r *http.Request) {
	res, err := h.service.GetAll(r.Context())
	if err != nil {
		h.respondWithError(w, err)
		return
	}
	api.WriteJSON(w, http.StatusOK, api.SuccessResponse("FAQs retrieved successfully", res))
}

// GetByID @Summary Get FAQ by ID
// @Tags FAQs
// @Param id path int true "FAQ ID"
// @Produce json
// @Success 200 {object} service.FAQResponse
// @Router /faqs/{id} [get]
func (h *FaqHandler) GetByID(w http.ResponseWriter, r *http.Request) {
	id, err := h.parseID(r)
	if err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid ID format"))
		return
	}

	res, err := h.service.GetByID(r.Context(), id)
	if err != nil {
		h.respondWithError(w, err)
		return
	}
	api.WriteJSON(w, http.StatusOK, api.SuccessResponse("FAQ retrieved successfully", res))
}

// Create @Summary Create a new FAQ
// @Tags FAQs
// @Accept json
// @Produce json
// @Param body body service.CreateFAQRequest true "FAQ Data"
// @Success 201 {object} api.MessageResponse
// @Router /faqs [post]
func (h *FaqHandler) Create(w http.ResponseWriter, r *http.Request) {
	var req service.CreateFAQRequest
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid JSON format"))
		return
	}

	if err := h.service.Create(r.Context(), req); err != nil {
		h.respondWithError(w, err)
		return
	}
	api.WriteJSON(w, http.StatusCreated, api.SuccessMessage("FAQ created successfully"))

}

// Update @Summary Update FAQ
// @Tags FAQs
// @Param id path int true "FAQ ID"
// @Param body body service.UpdateFAQRequest true "Update Data"
// @Accept json
// @Produce json
// @Success 200 {object} api.MessageResponse
// @Router /faqs/{id} [put]
func (h *FaqHandler) Update(w http.ResponseWriter, r *http.Request) {
	id, err := h.parseID(r)
	if err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid ID format"))
		return
	}

	var req service.UpdateFAQRequest
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid JSON format"))
		return
	}
	req.ID = id

	if err := h.service.Update(r.Context(), req); err != nil {
		h.respondWithError(w, err)
		return
	}
	api.WriteJSON(w, http.StatusOK, api.SuccessMessage("FAQ updated successfully"))
}

// Delete @Summary Delete FAQ
// @Tags FAQs
// @Param id path int true "FAQ ID"
// @Success 200 {object} api.MessageResponse
// @Router /faqs/{id} [delete]
func (h *FaqHandler) Delete(w http.ResponseWriter, r *http.Request) {
	id, err := h.parseID(r)
	if err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid ID format"))
		return
	}

	if err := h.service.Delete(r.Context(), id); err != nil {
		h.respondWithError(w, err)
		return
	}
	api.WriteJSON(w, http.StatusOK, api.SuccessMessage("FAQ deleted successfully"))
}

func (h *FaqHandler) parseID(r *http.Request) (uint, error) {
	idStr := chi.URLParam(r, "id")
	id, err := strconv.ParseUint(idStr, 10, 32)
	if err != nil {
		return 0, err
	}
	return uint(id), nil
}

func (h *FaqHandler) respondWithError(w http.ResponseWriter, err error) {
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
