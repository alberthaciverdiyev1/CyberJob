package http

import "time"

type CreateBannerRequest struct {
	ImageUrl       string    `json:"image_url" validate:"required,url"`
	Type           string    `json:"type" validate:"required,oneof=main sidebar popup"`
	Page           string    `json:"page" validate:"required"`
	ExpirationDate time.Time `json:"expiration_date" validate:"required,gt"`
}
type BannerResponse struct {
	ID             uint      `json:"id"`
	ImageUrl       string    `json:"image_url"`
	Type           string    `json:"type"`
	ExpirationDate time.Time `json:"expiration_date"`
}
