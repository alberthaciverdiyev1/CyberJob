package repository

import (
	"context"
	"errors"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/company/domain"
	"gorm.io/gorm"
)

type companyRepository struct {
	db *gorm.DB
}

func NewCompanyRepository(db *gorm.DB) domain.CompanyRepository {
	return &companyRepository{db: db}
}

func (r *companyRepository) Create(ctx context.Context, comp *domain.Company) error {
	return r.db.WithContext(ctx).Create(comp).Error
}

func (r *companyRepository) GetAll(ctx context.Context) ([]domain.Company, error) {
	var comps []domain.Company
	err := r.db.WithContext(ctx).Find(&comps).Error
	return comps, err
}

func (r *companyRepository) Details(ctx context.Context, id uint) (*domain.Company, error) {
	var comp domain.Company
	err := r.db.WithContext(ctx).First(&comp, id).Error
	if err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, nil
		}
		return nil, err
	}
	return &comp, nil
}

func (r *companyRepository) Update(ctx context.Context, comp *domain.Company) error {
	return r.db.WithContext(ctx).Save(comp).Error
}

func (r *companyRepository) Delete(ctx context.Context, id uint) error {
	return r.db.WithContext(ctx).Delete(&domain.Company{}, id).Error
}

func (r *companyRepository) Filter(ctx context.Context, filter domain.CompanyFilter) ([]domain.Company, error) {
	var comps []domain.Company

	query := r.db.WithContext(ctx).Model(&domain.Company{})

	if filter.Name != "" {
		query = query.Where("name ILIKE ?", "%"+filter.Name+"%")
	}

	if filter.Email != "" {
		query = query.Where("email = ?", filter.Email)
	}

	if filter.CategoryID != 0 {
		query = query.Where("category_id = ?", filter.CategoryID)
	}

	if filter.IsActive != nil {
		query = query.Where("is_active = ?", *filter.IsActive)
	}

	if filter.Limit > 0 {
		query = query.Limit(filter.Limit)
	} else {
		query = query.Limit(20)
	}

	query = query.Order("created_at DESC")

	err := query.Find(&comps).Error
	if err != nil {
		return nil, err
	}

	return comps, nil
}
