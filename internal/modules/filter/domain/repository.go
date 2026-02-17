package domain

import "context"

type FilterRepository interface {
	GetAll(ctx context.Context) ([]Filter, error)
	GetByID(ctx context.Context, id uint) (*Filter, error)
	Create(ctx context.Context, filter *Filter) error
	Update(ctx context.Context, filter *Filter) error
	Delete(ctx context.Context, id uint) error
}
