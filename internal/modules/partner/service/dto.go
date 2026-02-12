package service

type CreatePartnerRequest struct {
	Name  string `json:"name" validate:"required"`
	Image string `json:"image" validate:"required"`
	Link  string `json:"link" validate:"required"`
}

type UpdatePartnerRequest struct {
	ID    uint
	Name  string `json:"name"`
	Image string `json:"image"`
	Link  string `json:"link"`
}

type PartnerResponse struct {
	ID    uint   `json:"id"`
	Name  string `json:"name"`
	Image string `json:"image"`
	Link  string `json:"link"`
}
