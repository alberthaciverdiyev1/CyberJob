package domain

import "github.com/alberthaciverdiyev1/CyberJob/internal/platform/db"

type Category struct {
	db.BaseEntity
	Name     string     `gorm:"type:varchar(100);not null;uniqueIndex"`
	Icon     string     `gorm:"type:varchar(100)"`
	ParentID *uint      `gorm:"index"`
	Children []Category `gorm:"foreignKey:ParentID"`
}
