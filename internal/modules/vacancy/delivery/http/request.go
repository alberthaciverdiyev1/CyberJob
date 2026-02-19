package http

import "time"

type CreateVacancyRequest struct {
	Name         string    `json:"name"         validate:"required"`
	Requirements string    `json:"requirements" validate:"required"`
	Description  string    `json:"description"  validate:"required"`
	City         string    `json:"city"         validate:"required"`
	BannerImage  string    `json:"banner_image"`
	MinSalary    uint      `json:"min_salary"`
	MaxSalary    uint      `json:"max_salary"`
	MinAge       uint      `json:"min_age"`
	MaxAge       uint      `json:"max_age"`
	Email        string    `json:"email"        validate:"required,email"`
	CompanyID    uint      `json:"company_id"   validate:"required"`
	CategoryID   uint      `json:"category_id"  validate:"required"`
	FilterIDs    []uint    `json:"filter_ids"`
	ExpireDate   time.Time `json:"expire_date"  validate:"required"`
}

type UpdateVacancyRequest struct {
	Name         string    `json:"name"`
	Requirements string    `json:"requirements"`
	Description  string    `json:"description"`
	City         string    `json:"city"`
	BannerImage  string    `json:"banner_image"`
	MinSalary    uint      `json:"min_salary"`
	MaxSalary    uint      `json:"max_salary"`
	MinAge       uint      `json:"min_age"`
	MaxAge       uint      `json:"max_age"`
	Email        string    `json:"email"       validate:"omitempty,email"`
	CompanyID    uint      `json:"company_id"`
	CategoryID   uint      `json:"category_id"`
	FilterIDs    []uint    `json:"filter_ids"`
	ExpireDate   time.Time `json:"expire_date"`
}
