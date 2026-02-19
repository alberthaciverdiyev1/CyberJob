package http

import (
	"encoding/json"
	"net/http"
	"strconv"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/category/domain"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/api"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/validation"
	"github.com/go-chi/chi/v5"
)

type CategoryHandler struct {
	service domain.CategoryService
}

func NewCategoryHandler(s domain.CategoryService) *CategoryHandler {
	return &CategoryHandler{service: s}
}

// @Summary Create Category
// @Tags Categories
// @Param body body domain.CreateCategoryRequest true "Category Info"
// @Router /categories [post]
func (h *CategoryHandler) Create(w http.ResponseWriter, r *http.Request) {
	var req domain.CreateCategoryRequest
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid request format"))
		return
	}

	if errMsg := validation.ValidateStruct(req); errMsg != "" {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse(errMsg))
		return
	}

	if err := h.service.Create(r.Context(), req); err != nil {
		api.WriteJSON(w, http.StatusInternalServerError, api.ErrorResponse(err.Error()))
		return
	}
	api.WriteJSON(w, http.StatusCreated, api.SuccessMessage("Created"))
}

// @Summary List All (Hierarchical)
// @Tags Categories
// @Router /categories [get]
func (h *CategoryHandler) List(w http.ResponseWriter, r *http.Request) {
	categories, err := h.service.GetAllWithChildren(r.Context())
	if err != nil {
		api.WriteJSON(w, http.StatusInternalServerError, api.ErrorResponse(err.Error()))
		return
	}
	api.WriteJSON(w, http.StatusOK, api.SuccessResponse("OK", categories))
}

// @Summary List Simple (Flat)
// @Tags Categories
// @Router /categories/simple [get]
func (h *CategoryHandler) ListSimple(w http.ResponseWriter, r *http.Request) {
	categories, err := h.service.GetAll(r.Context())
	if err != nil {
		api.WriteJSON(w, http.StatusInternalServerError, api.ErrorResponse(err.Error()))
		return
	}
	api.WriteJSON(w, http.StatusOK, api.SuccessResponse("OK", categories))
}

// @Summary Get Category
// @Tags Categories
// @Param id path int true "Category ID"
// @Router /categories/{id} [get]
func (h *CategoryHandler) GetByID(w http.ResponseWriter, r *http.Request) {
	id, err := h.parseID(r)
	if err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse(err.Error()))
		return
	}

	category, err := h.service.GetByID(r.Context(), id)
	if err != nil {
		api.WriteJSON(w, http.StatusNotFound, api.ErrorResponse("Not found"))
		return
	}
	api.WriteJSON(w, http.StatusOK, api.SuccessResponse("OK", category))
}

// @Summary Update Category
// @Tags Categories
// @Param id path int true "Category ID"
// @Param body body domain.UpdateCategoryRequest true "Update Info"
// @Router /categories/{id} [put]
func (h *CategoryHandler) Update(w http.ResponseWriter, r *http.Request) {
	id, err := h.parseID(r)
	if err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse(err.Error()))
		return
	}

	var req domain.UpdateCategoryRequest
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid format"))
		return
	}

	req.ID = id
	if err := h.service.Update(r.Context(), req); err != nil {
		api.WriteJSON(w, http.StatusInternalServerError, api.ErrorResponse(err.Error()))
		return
	}
	api.WriteJSON(w, http.StatusOK, api.SuccessMessage("Updated"))
}

// @Summary Delete Category
// @Tags Categories
// @Param id path int true "Category ID"
// @Router /categories/{id} [delete]
func (h *CategoryHandler) Delete(w http.ResponseWriter, r *http.Request) {
	id, err := h.parseID(r)
	if err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse(err.Error()))
		return
	}

	if err := h.service.Delete(r.Context(), id); err != nil {
		api.WriteJSON(w, http.StatusInternalServerError, api.ErrorResponse(err.Error()))
		return
	}
	api.WriteJSON(w, http.StatusOK, api.SuccessMessage("Deleted"))
}

func (h *CategoryHandler) parseID(r *http.Request) (uint, error) {
	idStr := chi.URLParam(r, "id")
	id, err := strconv.ParseUint(idStr, 10, 32)
	if err != nil {
		return 0, err
	}
	return uint(id), nil
}
