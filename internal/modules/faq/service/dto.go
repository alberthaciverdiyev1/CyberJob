package service

type CreateFAQRequest struct {
	QuestionAz string `json:"question_az" validate:"required" formData:"question_az"`
	AnswerAz   string `json:"answer_az" validate:"required" formData:"answer_az"`

	QuestionEn string `json:"question_en" validate:"required" formData:"question_en"`
	AnswerEn   string `json:"answer_en" validate:"required" formData:"answer_en"`

	QuestionRu string `json:"question_ru" validate:"required" formData:"question_ru"`
	AnswerRu   string `json:"answer_ru" validate:"required" formData:"answer_ru"`
}

type UpdateFAQRequest struct {
	ID uint `json:"id" validate:"required"`

	QuestionAz string `json:"question_az" validate:"omitempty,min=3"`
	AnswerAz   string `json:"answer_az" validate:"omitempty"`

	QuestionEn string `json:"question_en" validate:"omitempty,min=3"`
	AnswerEn   string `json:"answer_en" validate:"omitempty"`

	QuestionRu string `json:"question_ru" validate:"omitempty,min=3"`
	AnswerRu   string `json:"answer_ru" validate:"omitempty"`
}

type FAQResponse struct {
	ID         uint   `json:"id"`
	QuestionAz string `json:"question_az"`
	AnswerAz   string `json:"answer_az"`

	QuestionEn string `json:"question_en"`
	AnswerEn   string `json:"answer_en"`

	QuestionRu string `json:"question_ru"`
	AnswerRu   string `json:"answer_ru"`

	CreatedAt string `json:"created_at"`
}
