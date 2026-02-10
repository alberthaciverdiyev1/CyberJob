package http

import (
	"encoding/json"
	"errors"
	"net/http"
	"strconv"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/company/domain"
	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/company/service"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/api"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/db"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/validation"
	"github.com/go-chi/chi/v5"
)

type CompanyHandler struct {
	service domain.CompanyService
}

func NewCompanyHandler(s domain.CompanyService) *CompanyHandler {
	return &CompanyHandler{service: s}
}

func (h *CompanyHandler) getID(r *http.Request) (uint, error) {
	idStr := chi.URLParam(r, "id")
	id, err := strconv.ParseUint(idStr, 10, 32)
	if err != nil {
		return 0, errors.New("invalid ID format")
	}
	return uint(id), nil
}

// Register POST /companies
// @Summary Register a new company
// @Description Creates a new company record with the provided information.
// @Tags Companies
// @Accept json
// @Produce json
// @Param company body CreateCompanyRequest true "Company Registration Info"
// @Success 201 {object} CompanyResponse
// @Failure 400 {object} string "Invalid request"
// @Failure 409 {object} string "Company already exists"
// @Failure 500 {object} string "Internal server error"
// @Router /companies [post]
func (h *CompanyHandler) Register(w http.ResponseWriter, r *http.Request) {
	var req CreateCompanyRequest
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid request format"))
		return
	}

	if errMsg := validation.ValidateStruct(req); errMsg != "" {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse(errMsg))
		return
	}

	comp := &domain.Company{
		Name:         req.Name,
		Email:        req.Email,
		Phone:        req.Phone,
		Address:      req.Address,
		ShortAddress: req.ShortAddress,
		About:        req.About,
		FoundingDate: req.FoundingDate,
		IsActive:     req.IsActive,
	}

	if err := h.service.Register(r.Context(), comp); err != nil {
		if errors.Is(err, service.ErrCompanyAlreadyExists) {
			api.WriteJSON(w, http.StatusConflict, api.ErrorResponse(err.Error()))
			return
		}
		api.WriteJSON(w, http.StatusInternalServerError, api.ErrorResponse(err.Error()))
		return
	}

	api.WriteJSON(w, http.StatusCreated, api.SuccessResponse("Company registered successfully", comp))
}

// List GET /companies
// @Summary List companies with filters
// @Description Returns a list of companies based on name, email, or category filters.
// @Tags Companies
// @Produce json
// @Param name query string false "Filter by Name"
// @Param email query string false "Filter by Email"
// @Param category_id query int false "Filter by Category ID"
// @Param limit query int false "Limit results"
// @Success 200 {array} CompanyResponse
// @Failure 500 {object} string "Internal server error"
// @Router /companies [get]
func (h *CompanyHandler) List(w http.ResponseWriter, r *http.Request) {
	q := r.URL.Query()

	limit, _ := strconv.Atoi(q.Get("limit"))
	catID, _ := strconv.Atoi(q.Get("category_id"))

	filter := domain.CompanyFilter{
		Name:       q.Get("name"),
		Email:      q.Get("email"),
		CategoryID: uint(catID),
		Limit:      limit,
	}

	companies, err := h.service.List(r.Context(), filter)
	if err != nil {
		api.WriteJSON(w, http.StatusInternalServerError, api.ErrorResponse(err.Error()))
		return
	}

	api.WriteJSON(w, http.StatusOK, api.SuccessResponse("Companies retrieved successfully", companies))
}

// GetByID GET /companies/{id}
// @Summary Get company by ID
// @Description Returns details of a single company.
// @Tags Companies
// @Produce json
// @Param id path int true "Company ID"
// @Success 200 {object} CompanyDetailsResponse
// @Failure 404 {object} string "Company not found"
// @Router /companies/{id} [get]
func (h *CompanyHandler) GetByID(w http.ResponseWriter, r *http.Request) {
	id, err := h.getID(r)
	if err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse(err.Error()))
		return
	}

	company, err := h.service.Details(r.Context(), id)
	if err != nil {
		if errors.Is(err, service.ErrCompanyNotFound) {
			api.WriteJSON(w, http.StatusNotFound, api.ErrorResponse(err.Error()))
			return
		}
		api.WriteJSON(w, http.StatusInternalServerError, api.ErrorResponse(err.Error()))
		return
	}

	api.WriteJSON(w, http.StatusOK, api.SuccessResponse("Company details retrieved", company))
}

// Update PUT /companies/{id}
// @Summary Update a company
// @Description Updates an existing company's information.
// @Tags Companies
// @Accept json
// @Produce json
// @Param id path int true "Company ID"
// @Param company body CreateCompanyRequest true "Company Update Data"
// @Success 200 {object} string "Company updated successfully"
// @Failure 404 {object} string "Company not found"
// @Router /companies/{id} [put]
func (h *CompanyHandler) Update(w http.ResponseWriter, r *http.Request) {
	id, err := h.getID(r)
	if err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse(err.Error()))
		return
	}

	var req CreateCompanyRequest
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid request format"))
		return
	}

	if errMsg := validation.ValidateStruct(req); errMsg != "" {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse(errMsg))
		return
	}

	comp := &domain.Company{
		BaseEntity:   db.BaseEntity{ID: id},
		Name:         req.Name,
		Email:        req.Email,
		Phone:        req.Phone,
		Address:      req.Address,
		ShortAddress: req.ShortAddress,
		About:        req.About,
		FoundingDate: req.FoundingDate,
		IsActive:     req.IsActive,
	}

	if err := h.service.Update(r.Context(), comp); err != nil {
		if errors.Is(err, service.ErrCompanyNotFound) {
			api.WriteJSON(w, http.StatusNotFound, api.ErrorResponse(err.Error()))
			return
		}
		api.WriteJSON(w, http.StatusInternalServerError, api.ErrorResponse(err.Error()))
		return
	}

	api.WriteJSON(w, http.StatusOK, api.SuccessResponse("Company updated successfully", nil))
}

// Delete DELETE /companies/{id}
// @Summary Delete a company
// @Description Removes a company from the database.
// @Tags Companies
// @Produce json
// @Param id path int true "Company ID"
// @Success 200 {object} string "Company deleted successfully"
// @Router /companies/{id} [delete]
func (h *CompanyHandler) Delete(w http.ResponseWriter, r *http.Request) {
	id, err := h.getID(r)
	if err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse(err.Error()))
		return
	}

	if err := h.service.Delete(r.Context(), id); err != nil {
		api.WriteJSON(w, http.StatusInternalServerError, api.ErrorResponse(err.Error()))
		return
	}

	api.WriteJSON(w, http.StatusOK, api.SuccessResponse("Company deleted successfully", nil))
}
