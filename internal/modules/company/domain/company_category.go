package domain

import (
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/db"
)

type CompanyCategory struct {
	db.BaseEntity
	Name string `gorm:"type:varchar(100);not null;uniqueIndex" json:"name"`

	Companies []Company `gorm:"foreignKey:CategoryID;constraint:OnUpdate:CASCADE,OnDelete:RESTRICT" json:"companies,omitempty"`
}
