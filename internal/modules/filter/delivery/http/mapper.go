package http

import (
	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/filter/domain"
)

// --- ADMIN MAPPERS ---

func MapToFullResponse(f domain.Filter) FilterFullResponse {
	return FilterFullResponse{
		ID:     f.ID,
		Key:    f.Key,
		NameAz: f.NameAz,
		NameEn: f.NameEn,
		NameRu: f.NameRu,
	}
}

func MapToFullResponseList(filters []domain.Filter) []FilterFullResponse {
	response := make([]FilterFullResponse, len(filters))
	for i, f := range filters {
		response[i] = MapToFullResponse(f)
	}
	return response
}

// --- USER MAPPERS ---

func MapToResponse(f domain.Filter, lang string) FilterResponse {
	name := f.NameAz

	switch lang {
	case "en":
		if f.NameEn != "" {
			name = f.NameEn
		}
	case "ru":
		if f.NameRu != "" {
			name = f.NameRu
		}
	}

	return FilterResponse{
		ID:   f.ID,
		Key:  f.Key,
		Name: name,
	}
}

func MapToResponseList(filters []domain.Filter, lang string) []FilterResponse {
	response := make([]FilterResponse, len(filters))
	for i, f := range filters {
		response[i] = MapToResponse(f, lang)
	}
	return response
}
