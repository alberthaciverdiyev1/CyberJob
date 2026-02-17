package domain

import "context"

type FilterService interface {
	GetAll(ctx context.Context) ([]Filter, error)
	GetByID(ctx context.Context, id uint) (*Filter, error)
	Create(ctx context.Context, params CreateFilterParams) error
	Update(ctx context.Context, params UpdateFilterParams) error
	Delete(ctx context.Context, id uint) error
}
