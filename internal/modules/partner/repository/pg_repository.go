package repository

import (
	"context"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/partner/domain"
	"gorm.io/gorm"
)

type partnerRepository struct {
	db *gorm.DB
}

func NewPartnerRepository(db *gorm.DB) domain.PartnerRepository {
	return &partnerRepository{db: db}
}

func (p partnerRepository) Create(ctx context.Context, partner domain.Partner) error {
	return p.db.WithContext(ctx).Create(&partner).Error
}

func (p partnerRepository) GetAll(ctx context.Context) ([]domain.Partner, error) {
	var partners []domain.Partner
	err := p.db.WithContext(ctx).Find(&partners).Error
	return partners, err
}

func (p partnerRepository) GetByID(ctx context.Context, id uint) (*domain.Partner, error) {
	var partner domain.Partner
	err := p.db.WithContext(ctx).First(&partner, id).Error
	if err != nil {
		return nil, err
	}
	return &partner, nil

}

func (p partnerRepository) Update(ctx context.Context, partner domain.Partner) error {
	return p.db.WithContext(ctx).Save(&partner).Error
}

func (p partnerRepository) Delete(ctx context.Context, id uint) error {
	return p.db.WithContext(ctx).Delete(&domain.Partner{}, id).Error
}
