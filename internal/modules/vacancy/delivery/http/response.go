package http

import "time"

type VacancyListResponse struct {
	ID         uint                    `json:"id"`
	Name       string                  `json:"name"`
	City       string                  `json:"city"`
	ViewCount  uint                    `json:"view_count"`
	MinSalary  uint                    `json:"min_salary"`
	MaxSalary  uint                    `json:"max_salary"`
	ExpireDate time.Time               `json:"expire_date"`
	Company    VacancyCompanyResponse  `json:"company"`
	Category   VacancyCategoryResponse `json:"category"`
}

type VacancyDetailResponse struct {
	ID           uint                    `json:"id"`
	Name         string                  `json:"name"`
	Requirements string                  `json:"requirements"`
	Description  string                  `json:"description"`
	City         string                  `json:"city"`
	ViewCount    uint                    `json:"view_count"`
	ExpireDate   time.Time               `json:"expire_date"`
	BannerImage  string                  `json:"banner_image"`
	MinSalary    uint                    `json:"min_salary"`
	MaxSalary    uint                    `json:"max_salary"`
	MinAge       uint                    `json:"min_age"`
	MaxAge       uint                    `json:"max_age"`
	Email        string                  `json:"email"`
	IsActive     bool                    `json:"is_active"`
	Company      VacancyCompanyResponse  `json:"company"`
	Category     VacancyCategoryResponse `json:"category"`
	Filters      []VacancyFilterResponse `json:"filters"`
}

type VacancyCompanyResponse struct {
	ID         uint   `json:"id"`
	Name       string `json:"name"`
	Image      string `json:"image,omitempty"`
	IsVerified bool   `json:"is_verified"`
}

type VacancyCategoryResponse struct {
	ID   uint   `json:"id"`
	Name string `json:"name"`
	Icon string `json:"icon"`
}

type VacancyFilterResponse struct {
	ID   uint   `json:"id"`
	Key  string `json:"key"`
	Name string `json:"name"`
}

type VacancyListResult struct {
	Data  []VacancyListResponse `json:"data"`
	Total int64                 `json:"total"`
	Page  int                   `json:"page"`
	Limit int                   `json:"limit"`
}
