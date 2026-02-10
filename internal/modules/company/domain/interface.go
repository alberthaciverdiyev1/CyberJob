package domain

import (
	"context"
)

// --- Repository ---
type CompanyCategoryRepository interface {
	Create(ctx context.Context, cat *CompanyCategory) error
	GetAll(ctx context.Context) ([]CompanyCategory, error)
	GetByID(ctx context.Context, id uint) (*CompanyCategory, error)
	Update(ctx context.Context, cat *CompanyCategory) error
	Delete(ctx context.Context, id uint) error
	GetByName(ctx context.Context, name string) (*CompanyCategory, error)
}

type CompanyRepository interface {
	Create(ctx context.Context, comp *Company) error
	GetAll(ctx context.Context) ([]Company, error)
	Details(ctx context.Context, id uint) (*Company, error)
	Update(ctx context.Context, comp *Company) error
	Delete(ctx context.Context, id uint) error
	Filter(ctx context.Context, filter CompanyFilter) ([]Company, error)
}

// --- Service ---
type CompanyCategoryService interface {
	CreateCategory(ctx context.Context, cat *CompanyCategory) error
	GetAllCategories(ctx context.Context) ([]CompanyCategory, error)
	GetCategoryByID(ctx context.Context, id uint) (*CompanyCategory, error)
	UpdateCategory(ctx context.Context, cat *CompanyCategory) error
	DeleteCategory(ctx context.Context, id uint) error
}

type CompanyService interface {
	Register(ctx context.Context, comp *Company) error

	List(ctx context.Context, filter CompanyFilter) ([]Company, error)

	Details(ctx context.Context, id uint) (*Company, error)

	Update(ctx context.Context, comp *Company) error

	Delete(ctx context.Context, id uint) error
}
