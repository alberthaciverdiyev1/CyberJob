package http

type CreateFilterRequest struct {
	Key    string `json:"key" validate:"required,min=2,max=50"`
	NameAz string `json:"name_az" validate:"required"`
	NameEn string `json:"name_en" validate:"required"`
	NameRu string `json:"name_ru" validate:"required"`
}

type UpdateFilterRequest struct {
	Key    string `json:"key" validate:"omitempty,min=2,max=50"`
	NameAz string `json:"name_az" validate:"omitempty"`
	NameEn string `json:"name_en" validate:"omitempty"`
	NameRu string `json:"name_ru" validate:"omitempty"`
}
