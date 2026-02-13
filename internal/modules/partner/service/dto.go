package service

import "io"

type CreatePartnerRequest struct {
	Name string `json:"name" validate:"required" formData:"name"`
	Link string `json:"link" validate:"required" formData:"link"`

	Image io.Reader `json:"-" swaggerignore:"true"`
	Ext   string    `json:"-" swaggerignore:"true"`
}

type UpdatePartnerRequest struct {
	Name  *string   `json:"name" formData:"name"`
	Link  *string   `json:"link" formData:"link"`
	Image io.Reader `json:"-" swaggerignore:"true"`
	Ext   *string   `json:"-" swaggerignore:"true"`
}

type PartnerResponse struct {
	ID    uint   `json:"id"`
	Name  string `json:"name"`
	Link  string `json:"link"`
	Image string `json:"image"`
}
