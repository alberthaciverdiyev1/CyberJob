package repository

import (
	"context"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/filter/domain"
	"gorm.io/gorm"
)

type filterRepository struct {
	db *gorm.DB
}

func NewFilterRepository(db *gorm.DB) domain.FilterRepository {
	return &filterRepository{db: db}
}

func (f filterRepository) GetAll(ctx context.Context) ([]domain.Filter, error) {
	var filters []domain.Filter
	err := f.db.WithContext(ctx).Find(&filters).Error
	return filters, err

}

func (f filterRepository) GetByID(ctx context.Context, id uint) (*domain.Filter, error) {
	var filter domain.Filter
	err := f.db.WithContext(ctx).First(&filter, id).Error
	if err != nil {
		return nil, err
	}
	return &filter, nil
}

func (f filterRepository) Create(ctx context.Context, filter *domain.Filter) error {
	return f.db.WithContext(ctx).Create(filter).Error
}

func (f filterRepository) Update(ctx context.Context, filter *domain.Filter) error {
	return f.db.WithContext(ctx).Save(filter).Error
}

func (f filterRepository) Delete(ctx context.Context, id uint) error {
	return f.db.WithContext(ctx).Delete(&domain.Filter{}, id).Error
}
