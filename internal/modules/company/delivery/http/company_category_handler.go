package http

import (
	"encoding/json"
	"net/http"
	"strconv"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/company/domain"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/api"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/db"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/validation"
	"github.com/go-chi/chi/v5"
)

type CompanyCategoryHandler struct {
	service domain.CompanyCategoryService
}

func NewCompanyCategoryHandler(s domain.CompanyCategoryService) *CompanyCategoryHandler {
	return &CompanyCategoryHandler{service: s}
}

// Create POST /company-categories
// @Summary Create a new company category
// @Description Creates a new company category record and saves it to the database.
// @Tags Company Categories
// @Accept json
// @Produce json
// @Param category body CreateCompanyCategoryRequest true "Category Creation Info"
// @Success 201 {object} CompanyCategoryResponse
// @Failure 400 {object} string "Invalid request"
// @Failure 500 {object} string "Internal server error"
// @Router /company-categories [post]
func (h *CompanyCategoryHandler) Create(w http.ResponseWriter, r *http.Request) {
	var req CreateCompanyCategoryRequest
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid request format"))
		return
	}

	if errMsg := validation.ValidateStruct(req); errMsg != "" {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse(errMsg))
		return
	}

	cat := &domain.CompanyCategory{
		Name: req.Name,
	}

	if err := h.service.CreateCategory(r.Context(), cat); err != nil {
		api.WriteJSON(w, http.StatusInternalServerError, api.ErrorResponse(err.Error()))
		return
	}

	resData := CompanyCategoryResponse{
		ID:   cat.ID,
		Name: cat.Name,
	}

	api.WriteJSON(w, http.StatusCreated, api.SuccessResponse("Company Category created successfully", resData))
}

// List GET /company-categories
// @Summary List all company categories
// @Description Returns a list of all company categories.
// @Tags Company Categories
// @Produce json
// @Success 200 {array} CompanyCategoryResponse
// @Failure 500 {object} string "Internal server error"
// @Router /company-categories [get]
func (h *CompanyCategoryHandler) List(w http.ResponseWriter, r *http.Request) {
	categories, err := h.service.GetAllCategories(r.Context())
	if err != nil {
		api.WriteJSON(w, http.StatusInternalServerError, api.ErrorResponse(err.Error()))
		return
	}

	var resData []CompanyCategoryResponse
	for _, b := range categories {
		resData = append(resData, CompanyCategoryResponse{
			ID:   b.ID,
			Name: b.Name,
		})
	}

	api.WriteJSON(w, http.StatusOK, api.SuccessResponse("Categories retrieved successfully", resData))
}

// GetCategoryByID GET /company-categories/{id}
// @Summary Get company category by ID
// @Description Returns detail for a single company category.
// @Tags Company Categories
// @Produce json
// @Param id path int true "Category ID"
// @Success 200 {object} CompanyCategoryResponse
// @Failure 404 {object} string "Category not found"
// @Router /company-categories/{id} [get]
func (h *CompanyCategoryHandler) GetCategoryByID(w http.ResponseWriter, r *http.Request) {
	idStr := chi.URLParam(r, "id")
	id, err := strconv.ParseUint(idStr, 10, 32)
	if err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid ID format"))
		return
	}

	category, err := h.service.GetCategoryByID(r.Context(), uint(id))
	if err != nil {
		api.WriteJSON(w, http.StatusNotFound, api.ErrorResponse("Category not found"))
		return
	}

	resData := CompanyCategoryResponse{
		ID:   category.ID,
		Name: category.Name,
	}

	api.WriteJSON(w, http.StatusOK, api.SuccessResponse("Category details retrieved", resData))
}

// Update PUT /company-categories/{id}
// @Summary Update a company category
// @Description Updates an existing company category record.
// @Tags Company Categories
// @Accept json
// @Produce json
// @Param id path int true "Category ID"
// @Param category body CreateCompanyCategoryRequest true "Category Update Data"
// @Success 200 {object} string "Category updated successfully"
// @Router /company-categories/{id} [put]
func (h *CompanyCategoryHandler) Update(w http.ResponseWriter, r *http.Request) {
	idStr := chi.URLParam(r, "id")
	id, err := strconv.ParseUint(idStr, 10, 32)
	if err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid ID format"))
		return
	}

	var req CreateCompanyCategoryRequest
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid request format"))
		return
	}

	if errMsg := validation.ValidateStruct(req); errMsg != "" {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse(errMsg))
		return
	}

	cat := &domain.CompanyCategory{
		BaseEntity: db.BaseEntity{ID: uint(id)},
		Name:       req.Name,
	}

	if err := h.service.UpdateCategory(r.Context(), cat); err != nil {
		api.WriteJSON(w, http.StatusInternalServerError, api.ErrorResponse(err.Error()))
		return
	}

	api.WriteJSON(w, http.StatusOK, api.SuccessResponse("Company Category updated successfully", nil))
}

// Delete DELETE /company-categories/{id}
// @Summary Delete a company category
// @Description Removes a company category from the system.
// @Tags Company Categories
// @Produce json
// @Param id path int true "Category ID"
// @Success 200 {object} string "Category deleted successfully"
// @Router /company-categories/{id} [delete]
func (h *CompanyCategoryHandler) Delete(w http.ResponseWriter, r *http.Request) {
	idStr := chi.URLParam(r, "id")
	id, err := strconv.ParseUint(idStr, 10, 32)
	if err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid ID format"))
		return
	}

	if err := h.service.DeleteCategory(r.Context(), uint(id)); err != nil {
		api.WriteJSON(w, http.StatusInternalServerError, api.ErrorResponse(err.Error()))
		return
	}

	api.WriteJSON(w, http.StatusOK, api.SuccessResponse("Company Category deleted successfully", nil))
}
