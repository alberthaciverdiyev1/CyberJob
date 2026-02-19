package domain

import "time"

type CreateVacancyParams struct {
	Name         string
	Requirements string
	Description  string
	City         string
	BannerImage  string
	MinSalary    uint
	MaxSalary    uint
	MinAge       uint
	MaxAge       uint
	Email        string
	CompanyID    uint
	CategoryID   uint
	FilterIDs    []uint
	ExpireDate   time.Time
}

type UpdateVacancyParams struct {
	ID           uint
	Name         string
	Requirements string
	Description  string
	City         string
	BannerImage  string
	MinSalary    uint
	MaxSalary    uint
	MinAge       uint
	MaxAge       uint
	Email        string
	CompanyID    uint
	CategoryID   uint
	FilterIDs    []uint
	ExpireDate   time.Time
}

type VacancyFilterParams struct {
	Search     string
	CategoryID uint
	CompanyID  uint
	FilterIDs  []uint
	City       string
	MinSalary  uint
	MaxSalary  uint
	IsActive   *bool
	Page       int
	Limit      int
}
