package repository

import (
	"context"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/banners/domain"
	"gorm.io/gorm"
)

type bannerRepository struct {
	db *gorm.DB
}

func NewBannerRepository(db *gorm.DB) domain.BannerRepository {
	return &bannerRepository{db: db}
}

func (r *bannerRepository) GetByID(ctx context.Context, id uint) (*domain.Banner, error) {
	var banner domain.Banner
	err := r.db.WithContext(ctx).First(&banner, id).Error
	if err != nil {
		return nil, err
	}
	return &banner, nil
}

func (r *bannerRepository) Create(ctx context.Context, banner *domain.Banner) error {
	return r.db.WithContext(ctx).Create(banner).Error
}

func (r *bannerRepository) GetAll(ctx context.Context) ([]domain.Banner, error) {
	var banners []domain.Banner
	err := r.db.WithContext(ctx).Find(&banners).Error
	return banners, err
}

func (r *bannerRepository) Update(ctx context.Context, banner *domain.Banner) error {
	return r.db.WithContext(ctx).Save(banner).Error
}

func (r *bannerRepository) Delete(ctx context.Context, id uint) error {
	return r.db.WithContext(ctx).Delete(&domain.Banner{}, id).Error
}
