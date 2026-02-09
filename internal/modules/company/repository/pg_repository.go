package repository

import (
	"context"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/company/domain"
	"gorm.io/gorm"
)

type pgRepository struct {
	db *gorm.DB
}

func NewPgRepository(db *gorm.DB) domain.CategoryRepository {
	return &pgRepository{db: db}
}

// --- CategoryRepository Implementation ---

func (r *pgRepository) Create(ctx context.Context, cat *domain.CompanyCategory) error {
	return r.db.WithContext(ctx).Create(cat).Error
}

func (r *pgRepository) GetAll(ctx context.Context) ([]domain.CompanyCategory, error) {
	var cats []domain.CompanyCategory
	err := r.db.WithContext(ctx).Find(&cats).Error
	return cats, err
}

func (r *pgRepository) GetByID(ctx context.Context, id uint) (*domain.CompanyCategory, error) {
	var cat domain.CompanyCategory
	err := r.db.WithContext(ctx).First(&cat, id).Error
	return &cat, err
}

func (r *pgRepository) Update(ctx context.Context, cat *domain.CompanyCategory) error {
	return r.db.WithContext(ctx).Save(cat).Error
}

func (r *pgRepository) Delete(ctx context.Context, id uint) error {
	return r.db.WithContext(ctx).Delete(&domain.CompanyCategory{}, id).Error
}
func (r *pgRepository) GetByName(ctx context.Context, name string) (*domain.CompanyCategory, error) {
	var cat domain.CompanyCategory
	err := r.db.WithContext(ctx).Where("name = ?", name).First(&cat).Error
	if err != nil {
		return nil, err
	}
	return &cat, nil
}
