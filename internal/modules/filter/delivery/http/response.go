package http

type FilterResponse struct {
	ID   uint   `json:"id"`
	Key  string `json:"key"`
	Name string `json:"name"`
}

type FilterFullResponse struct {
	ID     uint   `json:"id"`
	Key    string `json:"key"`
	NameAz string `json:"name_az"`
	NameEn string `json:"name_en"`
	NameRu string `json:"name_ru"`
}
