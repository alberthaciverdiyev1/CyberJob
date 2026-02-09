package domain

import (
	"time"

	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/db"
)

type Company struct {
	db.BaseEntity
	Image        string     `gorm:"type:varchar(255)" json:"image"`
	BannerImage  string     `gorm:"type:varchar(255)" json:"banner_image"`
	Name         string     `gorm:"type:varchar(255);not null" json:"name"`
	Email        string     `gorm:"type:varchar(100);uniqueIndex" json:"email"`
	Phone        string     `gorm:"type:varchar(20)" json:"phone"`
	Address      string     `gorm:"type:text" json:"address"`
	ShortAddress string     `gorm:"type:varchar(150)" json:"short_address"`
	IsActive     bool       `gorm:"default:true" json:"is_active"`
	IsVerified   bool       `gorm:"default:false" json:"is_verified"`
	FoundingDate *time.Time `json:"founding_date"`
	About        string     `gorm:"type:text" json:"about"`

	CategoryID uint `gorm:"not null" json:"category_id"`
}
