package domain

import "context"

type VacancyRepository interface {
	GetAll(ctx context.Context, params VacancyFilterParams) ([]Vacancy, error)
	GetByID(ctx context.Context, id uint) (*Vacancy, error)
	Create(ctx context.Context, vacancy *Vacancy) error
	Update(ctx context.Context, vacancy *Vacancy) error
	Delete(ctx context.Context, id uint) error
}
