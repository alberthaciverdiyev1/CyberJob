package domain

type Partner struct {
	Name  string `gorm:"type:varchar(255)"`
	Image string `gorm:"type:varchar(255)"`
	Link  string `gorm:"type:varchar(100)"`
}
