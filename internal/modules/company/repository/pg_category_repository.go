package repository

import (
	"context"
	"errors"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/company/domain"
	"gorm.io/gorm"
)

type companyCategoryRepository struct {
	db *gorm.DB
}

func NewCompanyCategoryRepository(db *gorm.DB) domain.CompanyCategoryRepository {
	return &companyCategoryRepository{db: db}
}

// --- CategoryRepository Implementation ---

func (r *companyCategoryRepository) Create(ctx context.Context, cat *domain.CompanyCategory) error {
	return r.db.WithContext(ctx).Create(cat).Error
}

func (r *companyCategoryRepository) GetAll(ctx context.Context) ([]domain.CompanyCategory, error) {
	var cats []domain.CompanyCategory
	err := r.db.WithContext(ctx).Find(&cats).Error
	return cats, err
}

func (r *companyCategoryRepository) GetByID(ctx context.Context, id uint) (*domain.CompanyCategory, error) {
	var cat domain.CompanyCategory
	err := r.db.WithContext(ctx).First(&cat, id).Error
	if err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, nil
		}
		return nil, err
	}
	return &cat, nil
}

func (r *companyCategoryRepository) Update(ctx context.Context, cat *domain.CompanyCategory) error {

	return r.db.WithContext(ctx).Save(cat).Error
}

func (r *companyCategoryRepository) Delete(ctx context.Context, id uint) error {
	return r.db.WithContext(ctx).Delete(&domain.CompanyCategory{}, id).Error
}

func (r *companyCategoryRepository) GetByName(ctx context.Context, name string) (*domain.CompanyCategory, error) {
	var cat domain.CompanyCategory
	err := r.db.WithContext(ctx).Where("name = ?", name).First(&cat).Error
	if err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, nil
		}
		return nil, err
	}
	return &cat, nil
}
