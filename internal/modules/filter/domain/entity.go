package domain

import (
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/db"
)

type Filter struct {
	db.BaseEntity
	Key    string `gorm:"type:varchar(100)"`
	NameAz string `gorm:"type:varchar(100)"`
	NameEn string `gorm:"type:varchar(100)"`
	NameRu string `gorm:"type:varchar(100)"`
}
