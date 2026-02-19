package domain

import (
	"time"

	category "github.com/alberthaciverdiyev1/CyberJob/internal/modules/category/domain"
	company "github.com/alberthaciverdiyev1/CyberJob/internal/modules/company/domain"
	filter "github.com/alberthaciverdiyev1/CyberJob/internal/modules/filter/domain"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/db"
)

type Vacancy struct {
	db.BaseEntity
	Name         string    `gorm:"not null;type:varchar(255)"`
	Requirements string    `gorm:"type:text"`
	Description  string    `gorm:"type:text"`
	City         string    `gorm:"not null;type:varchar(100)"`
	ViewCount    uint      `gorm:"default:0"`
	ExpireDate   time.Time `gorm:"not null"`
	BannerImage  string    `gorm:"type:text"`
	MinSalary    uint      `gorm:"type:int"`
	MaxSalary    uint      `gorm:"type:int"`
	MinAge       uint      `gorm:"type:int"`
	MaxAge       uint      `gorm:"type:int"`
	Email        string    `gorm:"type:varchar(100)"`
	CompanyID    uint      `gorm:"not null;index"`
	Company      *company.Company
	CategoryID   uint `gorm:"not null;index"`
	Category     *category.Category
	IsActive     bool            `gorm:"default:true"`
	IsPremium    bool            `gorm:"default:false"`
	IsBringTop   bool            `gorm:"default:false"`
	Filters      []filter.Filter `gorm:"many2many:vacancy_filters;"`
}
