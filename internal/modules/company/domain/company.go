package domain

import (
	"time"

	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/db"
)

type Company struct {
	db.BaseEntity
	Image        string `gorm:"type:varchar(255)"`
	BannerImage  string `gorm:"type:varchar(255)"`
	Name         string `gorm:"type:varchar(255);not null"`
	Email        string `gorm:"type:varchar(100);uniqueIndex"`
	Phone        string `gorm:"type:varchar(20)"`
	Address      string `gorm:"type:text"`
	ShortAddress string `gorm:"type:varchar(150)"`
	IsActive     bool   `gorm:"default:true"`
	IsVerified   bool   `gorm:"default:false"`
	FoundingDate *time.Time
	About        string `gorm:"type:text"`

	CategoryID uint `gorm:"not null"`
}
type CompanyFilter struct {
	CategoryID uint
	IsActive   *bool
	Name       string
	Limit      int
	Email      string
}
