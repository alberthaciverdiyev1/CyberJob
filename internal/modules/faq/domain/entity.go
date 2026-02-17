package domain

import "github.com/alberthaciverdiyev1/CyberJob/internal/platform/db"

type FAQ struct {
	db.BaseEntity
	QuestionAz string `gorm:"type:varchar(255);not null" json:"question_az"`
	QuestionEn string `gorm:"type:varchar(255);not null" json:"question_en"`
	QuestionRu string `gorm:"type:varchar(255);not null" json:"question_ru"`

	AnswerAz string `gorm:"type:text" json:"answer_az"`
	AnswerEn string `gorm:"type:text" json:"answer_en"`
	AnswerRu string `gorm:"type:text" json:"answer_ru"`
}
