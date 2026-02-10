package http

type CreateCompanyRequest struct {
	Image       string `json:"image" validate:"omitempty"`
	BannerImage string `json:"banner_image" validate:"omitempty"`

	Name         string `json:"name" validate:"required"`
	Email        string `json:"email" validate:"omitempty,email"`
	Phone        string `json:"phone" validate:"omitempty,numeric"`
	Address      string `json:"address" validate:"omitempty"`
	ShortAddress string `json:"short_address" validate:"omitempty"`
	About        string `json:"about" validate:"omitempty"`
	FoundingDate string `json:"founding_date" validate:"omitempty,datetime=2006-01-02" example:"2026-02-10"`
	IsActive     bool   `json:"is_active" validate:"omitempty"`
	IsVerified   bool   `json:"is_verified" validate:"omitempty"`
}

type CompanyResponse struct {
	ID         uint   `json:"id"`
	Name       string `json:"name"`
	Image      string `json:"image,omitempty"`
	ImageUrl   string `json:"image_url,omitempty"`
	IsVerified bool   `json:"is_verified"`
}
type CompanyDetailsResponse struct {
	ID           uint   `json:"id"`
	Name         string `json:"name"`
	Email        string `json:"email,omitempty"`
	Phone        string `json:"phone,omitempty"`
	Image        string `json:"image,omitempty"`
	BannerImage  string `json:"banner_image,omitempty"`
	Address      string `json:"address,omitempty"`
	ShortAddress string `json:"short_address,omitempty"`
	About        string `json:"about,omitempty"`
	FoundingDate string `json:"founding_date" validate:"omitempty,datetime=2006-01-02" example:"2026-02-10"`
	IsActive     bool   `json:"is_active"`
	IsVerified   bool   `json:"is_verified"`
}

type CreateCompanyCategoryRequest struct {
	Name string `json:"name" validate:"required"`
}

type CompanyCategoryResponse struct {
	ID   uint   `json:"id"`
	Name string `json:"name"`
}
