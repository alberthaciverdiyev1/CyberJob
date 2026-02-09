package http

import (
	"encoding/json"
	"net/http"
	"strconv"

	"github.com/alberthaciverdiyev1/CyberJob/internal/banners/domain"
	"github.com/alberthaciverdiyev1/CyberJob/internal/pkg/utils"
	"github.com/go-chi/chi/v5"
)

type BannerHandler struct {
	service domain.BannerService
}

func NewBannerHandler(s domain.BannerService) *BannerHandler {
	return &BannerHandler{service: s}
}

// Create POST /banners
// @Summary Create a new banner
// @Description Creates a new banner record and saves it to the database. Response is wrapped in APIResponse.
// @Tags Banners
// @Accept json
// @Produce json
// @Param banner body CreateBannerRequest true "Banner Creation Info"
// @Success 201 {object} BannerResponse "Banner created successfully"
// @Failure 400 {object} string "Invalid request"
// @Failure 500 {object} string "Internal server error"
// @Router /banners [post]
func (h *BannerHandler) Create(w http.ResponseWriter, r *http.Request) {
	var req CreateBannerRequest
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		utils.WriteJSON(w, http.StatusBadRequest, utils.ErrorResponse("Invalid request format"))
		return
	}

	if errMsg := utils.ValidateStruct(req); errMsg != "" {
		utils.WriteJSON(w, http.StatusBadRequest, utils.ErrorResponse(errMsg))
		return
	}

	banner := &domain.Banner{
		ImageUrl:       req.ImageUrl,
		Type:           req.Type,
		Page:           req.Page,
		ExpirationDate: req.ExpirationDate,
	}

	if err := h.service.CreateBanner(r.Context(), banner); err != nil {
		utils.WriteJSON(w, http.StatusInternalServerError, utils.ErrorResponse(err.Error()))
		return
	}

	resData := BannerResponse{
		ID:             banner.ID,
		ImageUrl:       banner.ImageUrl,
		Type:           banner.Type,
		ExpirationDate: banner.ExpirationDate,
	}

	utils.WriteJSON(w, http.StatusCreated, utils.SuccessResponse("Banner created successfully", resData))
}

// List GET /banners
// @Summary List all banners
// @Description Returns a list of all active banners. Response is wrapped in APIResponse.
// @Tags Banners
// @Produce json
// @Success 200 {array} BannerResponse "Banners retrieved successfully"
// @Failure 500 {object} string "Internal server error"
// @Router /banners [get]
func (h *BannerHandler) List(w http.ResponseWriter, r *http.Request) {
	banners, err := h.service.GetActiveBanners(r.Context())
	if err != nil {
		utils.WriteJSON(w, http.StatusInternalServerError, utils.ErrorResponse(err.Error()))
		return
	}

	var resData []BannerResponse
	for _, b := range banners {
		resData = append(resData, BannerResponse{
			ID:             b.ID,
			ImageUrl:       b.ImageUrl,
			Type:           b.Type,
			ExpirationDate: b.ExpirationDate,
		})
	}

	utils.WriteJSON(w, http.StatusOK, utils.SuccessResponse("Banners retrieved successfully", resData))
}

// GetByID GET /banners/{id}
// @Summary Get banner by ID
// @Description Returns detail for a single banner. Response is wrapped in APIResponse.
// @Tags Banners
// @Produce json
// @Param id path int true "Banner ID"
// @Success 200 {object} BannerResponse "Banner details retrieved"
// @Failure 404 {object} string "Banner not found"
// @Router /banners/{id} [get]
func (h *BannerHandler) GetByID(w http.ResponseWriter, r *http.Request) {
	idStr := chi.URLParam(r, "id")
	id, err := strconv.ParseUint(idStr, 10, 32)
	if err != nil {
		utils.WriteJSON(w, http.StatusBadRequest, utils.ErrorResponse("Invalid ID format"))
		return
	}

	banner, err := h.service.GetBannerDetail(r.Context(), uint(id))
	if err != nil {
		utils.WriteJSON(w, http.StatusNotFound, utils.ErrorResponse("Banner not found"))
		return
	}

	resData := BannerResponse{
		ID:             banner.ID,
		ImageUrl:       banner.ImageUrl,
		Type:           banner.Type,
		ExpirationDate: banner.ExpirationDate,
	}

	utils.WriteJSON(w, http.StatusOK, utils.SuccessResponse("Banner details retrieved", resData))
}

// Update PUT /banners/{id}
// @Summary Update a banner
// @Description Updates an existing banner record. Response is wrapped in APIResponse.
// @Tags Banners
// @Accept json
// @Produce json
// @Param id path int true "Banner ID"
// @Param banner body CreateBannerRequest true "Banner Update Data"
// @Success 200 {object} string "Banner updated successfully"
// @Router /banners/{id} [put]
func (h *BannerHandler) Update(w http.ResponseWriter, r *http.Request) {
	idStr := chi.URLParam(r, "id")
	id, err := strconv.ParseUint(idStr, 10, 32)
	if err != nil {
		utils.WriteJSON(w, http.StatusBadRequest, utils.ErrorResponse("Invalid ID format"))
		return
	}

	var req CreateBannerRequest
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		utils.WriteJSON(w, http.StatusBadRequest, utils.ErrorResponse("Invalid request format"))
		return
	}

	if errMsg := utils.ValidateStruct(req); errMsg != "" {
		utils.WriteJSON(w, http.StatusBadRequest, utils.ErrorResponse(errMsg))
		return
	}

	banner := &domain.Banner{
		BaseEntity:     domain.BaseEntity{ID: uint(id)},
		ImageUrl:       req.ImageUrl,
		Type:           req.Type,
		Page:           req.Page,
		ExpirationDate: req.ExpirationDate,
	}

	if err := h.service.UpdateBanner(r.Context(), banner); err != nil {
		utils.WriteJSON(w, http.StatusInternalServerError, utils.ErrorResponse(err.Error()))
		return
	}

	utils.WriteJSON(w, http.StatusOK, utils.SuccessResponse("Banner updated successfully", nil))
}

// Delete DELETE /banners/{id}
// @Summary Delete a banner
// @Description Removes a banner from the system. Response is wrapped in APIResponse.
// @Tags Banners
// @Produce json
// @Param id path int true "Banner ID"
// @Success 200 {object} string "Banner deleted successfully"
// @Router /banners/{id} [delete]
func (h *BannerHandler) Delete(w http.ResponseWriter, r *http.Request) {
	idStr := chi.URLParam(r, "id")
	id, err := strconv.ParseUint(idStr, 10, 32)
	if err != nil {
		utils.WriteJSON(w, http.StatusBadRequest, utils.ErrorResponse("Invalid ID format"))
		return
	}

	if err := h.service.DeleteBanner(r.Context(), uint(id)); err != nil {
		utils.WriteJSON(w, http.StatusInternalServerError, utils.ErrorResponse(err.Error()))
		return
	}

	utils.WriteJSON(w, http.StatusOK, utils.SuccessResponse("Banner deleted successfully", nil))
}
