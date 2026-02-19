package http

import (
	"context"
	"errors"
	"log/slog"
	"net/http"
	"strconv"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/filter/domain"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/api"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/validation"
	"github.com/go-chi/chi/v5"
)

type contextKey string

const contextKeyRole contextKey = "role"

type FilterHandler struct {
	service domain.FilterService
	logger  *slog.Logger
}

func NewFilterHandler(s domain.FilterService, logger *slog.Logger) *FilterHandler {
	return &FilterHandler{
		service: s,
		logger:  logger,
	}
}

// GetAll @Summary List all Filters
// @Description Admin için tüm dilleri, User için lokalize edilmiş datayı döner
// @Tags Filters
// @Produce json
// @Success 200 {object} api.APIResponse[[]domain.FilterFullResponse] // Dizi olduğunu belirtmek için [] ekledik
// @Failure 500 {object} api.APIResponse[any]
// @Router /filters [get]
func (h *FilterHandler) GetAll(w http.ResponseWriter, r *http.Request) {
	filters, err := h.service.GetAll(r.Context())
	if err != nil {
		h.handleError(w, r, err)
		return
	}

	lang := r.Header.Get("Accept-Language")
	if lang == "" {
		lang = "az"
	}

	if h.isAdmin(r.Context()) {
		// Tip güvenliği ve Swagger uyumu için api.APIResponse yapısını doğrudan kullanıyoruz
		api.WriteJSON(w, http.StatusOK, api.SuccessResponse("Filters retrieved", MapToFullResponseList(filters)))
	} else {
		api.WriteJSON(w, http.StatusOK, api.SuccessResponse("Filters retrieved", MapToResponseList(filters, lang)))
	}
}

// GetByID @Summary Get Filter by ID
// @Description ID'ye göre tek bir filtreyi getirir
// @Tags Filters
// @Param id path int true "Filter ID"
// @Produce json
// @Success 200 {object} api.APIResponse[domain.FilterFullResponse]
// @Failure 400 {object} api.APIResponse[any]
// @Failure 404 {object} api.APIResponse[any]
// @Router /filters/{id} [get]
func (h *FilterHandler) GetByID(w http.ResponseWriter, r *http.Request) {
	id, err := h.parseID(r)
	if err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid ID"))
		return
	}

	filter, err := h.service.GetByID(r.Context(), id)
	if err != nil {
		h.handleError(w, r, err)
		return
	}

	lang := r.Header.Get("Accept-Language")
	if lang == "" {
		lang = "az"
	}

	if h.isAdmin(r.Context()) {
		api.WriteJSON(w, http.StatusOK, api.SuccessResponse("Filter found", MapToFullResponse(*filter)))
	} else {
		api.WriteJSON(w, http.StatusOK, api.SuccessResponse("Filter found", MapToResponse(*filter, lang)))
	}
}

// Create @Summary Create a new Filter
// @Description Yeni bir filtre oluşturur
// @Tags Filters
// @Accept json
// @Produce json
// @Param body body domain.CreateFilterParams true "Filter Data"
// @Success 201 {object} api.APIResponse[any]
// @Failure 400 {object} api.APIResponse[any]
// @Failure 500 {object} api.APIResponse[any]
// @Router /filters [post]
func (h *FilterHandler) Create(w http.ResponseWriter, r *http.Request) {
	var req CreateFilterRequest
	if err := api.ReadJSON(w, r, &req); err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse(err.Error()))
		return
	}

	if errMsg := validation.ValidateStruct(req); errMsg != "" {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse(errMsg))
		return
	}

	params := domain.CreateFilterParams{
		Key:    req.Key,
		NameAz: req.NameAz,
		NameEn: req.NameEn,
		NameRu: req.NameRu,
	}

	if err := h.service.Create(r.Context(), params); err != nil {
		h.handleError(w, r, err)
		return
	}

	api.WriteJSON(w, http.StatusCreated, api.SuccessMessage("Filter created successfully"))
}

// Update @Summary Update a Filter
// @Description Mevcut bir filtreyi günceller
// @Tags Filters
// @Accept json
// @Produce json
// @Param id path int true "Filter ID"
// @Param body body domain.UpdateFilterParams true "Update Data"
// @Success 200 {object} api.APIResponse[any]
// @Failure 400 {object} api.APIResponse[any]
// @Failure 404 {object} api.APIResponse[any]
// @Failure 500 {object} api.APIResponse[any]
// @Router /filters/{id} [put]
func (h *FilterHandler) Update(w http.ResponseWriter, r *http.Request) {
	id, err := h.parseID(r)
	if err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid ID"))
		return
	}

	var req UpdateFilterRequest
	if err := api.ReadJSON(w, r, &req); err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse(err.Error()))
		return
	}

	if errMsg := validation.ValidateStruct(req); errMsg != "" {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse(errMsg))
		return
	}

	params := domain.UpdateFilterParams{
		ID:     id,
		Key:    req.Key,
		NameAz: req.NameAz,
		NameEn: req.NameEn,
		NameRu: req.NameRu,
	}

	if err := h.service.Update(r.Context(), params); err != nil {
		h.handleError(w, r, err)
		return
	}

	api.WriteJSON(w, http.StatusOK, api.SuccessMessage("Filter updated successfully"))
}

// Delete @Summary Delete a Filter
// @Description ID'ye göre filtreyi siler
// @Tags Filters
// @Param id path int true "Filter ID"
// @Produce json
// @Success 200 {object} api.APIResponse[any]
// @Failure 400 {object} api.APIResponse[any]
// @Failure 404 {object} api.APIResponse[any]
// @Failure 500 {object} api.APIResponse[any]
// @Router /filters/{id} [delete]
func (h *FilterHandler) Delete(w http.ResponseWriter, r *http.Request) {
	id, err := h.parseID(r)
	if err != nil {
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse("Invalid ID"))
		return
	}

	if err := h.service.Delete(r.Context(), id); err != nil {
		h.handleError(w, r, err)
		return
	}

	api.WriteJSON(w, http.StatusOK, api.SuccessMessage("Filter deleted successfully"))
}

// parseID - URL'den ID'yi okur ve uint'e çevirir
func (h *FilterHandler) parseID(r *http.Request) (uint, error) {
	id, err := strconv.ParseUint(chi.URLParam(r, "id"), 10, 32)
	if err != nil || id == 0 {
		return 0, errors.New("invalid id")
	}
	return uint(id), nil
}

// isAdmin - Context'ten rolü okur, query param'a güvenmez
func (h *FilterHandler) isAdmin(ctx context.Context) bool {
	role, _ := ctx.Value(contextKeyRole).(string)
	return role == "admin"
}

// handleError - Service hatalarını HTTP status kodlarına çevirir ve loglar
func (h *FilterHandler) handleError(w http.ResponseWriter, r *http.Request, err error) {
	switch {
	case errors.Is(err, domain.ErrFilterNotFound):
		api.WriteJSON(w, http.StatusNotFound, api.ErrorResponse(err.Error()))
	case errors.Is(err, domain.ErrInvalidFilterID):
		api.WriteJSON(w, http.StatusBadRequest, api.ErrorResponse(err.Error()))
	default:
		h.logger.Error("unexpected error",
			"method", r.Method,
			"path", r.URL.Path,
			"error", err,
		)
		api.WriteJSON(w, http.StatusInternalServerError, api.ErrorResponse("An internal error occurred"))
	}
}
