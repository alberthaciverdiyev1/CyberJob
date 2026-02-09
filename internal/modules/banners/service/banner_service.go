package service

import (
	"context"
	"errors"
	"time"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/banners/domain"
)

type bannerService struct {
	repo domain.BannerRepository
}

func NewBannerService(repo domain.BannerRepository) domain.BannerService {
	return &bannerService{repo: repo}
}

func (s *bannerService) CreateBanner(ctx context.Context, banner *domain.Banner) error {
	if banner.ExpirationDate.Before(time.Now()) {
		return errors.New("geçerlilik tarihi bugünden önce olamaz")
	}
	return s.repo.Create(ctx, banner)
}

func (s *bannerService) GetActiveBanners(ctx context.Context) ([]domain.Banner, error) {

	return s.repo.GetAll(ctx)
}

func (s *bannerService) GetBannerDetail(ctx context.Context, id uint) (*domain.Banner, error) {
	banner, err := s.repo.GetByID(ctx, id)
	if err != nil {
		return nil, errors.New("istenen banner bulunamadı")
	}
	return banner, nil
}

func (s *bannerService) UpdateBanner(ctx context.Context, banner *domain.Banner) error {
	_, err := s.repo.GetByID(ctx, banner.ID)
	if err != nil {
		return errors.New("güncellenecek banner sistemde kayıtlı değil")
	}

	return s.repo.Update(ctx, banner)
}

func (s *bannerService) DeleteBanner(ctx context.Context, id uint) error {
	_, err := s.repo.GetByID(ctx, id)
	if err != nil {
		return errors.New("silinmek istenen banner zaten mevcut değil")
	}

	return s.repo.Delete(ctx, id)
}

func (s *bannerService) PublishBanner(ctx context.Context, banner *domain.Banner) error {
	banner.CreatedAt = time.Now()
	return s.repo.Create(ctx, banner)
}
