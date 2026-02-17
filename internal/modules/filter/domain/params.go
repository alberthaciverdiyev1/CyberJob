package domain

type UpdateFilterParams struct {
	ID     uint
	Key    string
	NameAz string
	NameEn string
	NameRu string
}

type CreateFilterParams struct {
	Key    string
	NameAz string
	NameEn string
	NameRu string
}
