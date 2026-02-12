package domain

import "github.com/alberthaciverdiyev1/CyberJob/internal/platform/db"

type Partner struct {
	db.BaseEntity
	Name  string `gorm:"type:varchar(255)"`
	Image string `gorm:"type:varchar(255)"`
	Link  string `gorm:"type:varchar(100)"`
}
