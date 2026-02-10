package domain

import "context"

type CategoryRepository interface {
	Create(ctx context.Context, cat *Category) error
	GetAll(ctx context.Context) ([]Category, error)
	GetAllWithChildren(ctx context.Context) ([]Category, error)
	GetByID(ctx context.Context, id uint) (*Category, error)
	GetByName(ctx context.Context, name string) (*Category, error)
	Update(ctx context.Context, cat *Category) error
	Delete(ctx context.Context, id uint) error
}

type CategoryService interface {
	Create(ctx context.Context, req CreateCategoryRequest) error
	GetAll(ctx context.Context) ([]CategoryResponse, error)
	GetAllWithChildren(ctx context.Context) ([]CategoryResponse, error)
	GetByID(ctx context.Context, id uint) (*CategoryResponse, error)
	Update(ctx context.Context, req UpdateCategoryRequest) error
	Delete(ctx context.Context, id uint) error
}
