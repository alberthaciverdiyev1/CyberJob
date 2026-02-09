package service

import (
	"context"
	"errors"
	"time"

	"github.com/alberthaciverdiyev1/CyberJob/internal/banners/domain"
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

	// Güncelleme öncesi iş kuralları buraya yazılır.
	return s.repo.Update(ctx, banner)
}

// DeleteBanner: ID üzerinden banner siler.
func (s *bannerService) DeleteBanner(ctx context.Context, id uint) error {
	// Kontrol: Silinmek istenen kayıt var mı?
	_, err := s.repo.GetByID(ctx, id)
	if err != nil {
		return errors.New("silinmek istenen banner zaten mevcut değil")
	}

	return s.repo.Delete(ctx, id)
}

// PublishBanner: Interface'de tanımlı olan özel yayınlama mantığı.
func (s *bannerService) PublishBanner(ctx context.Context, banner *domain.Banner) error {
	// Örn: Yayına almadan önce son kontrolleri yap ve kaydet.
	banner.CreatedAt = time.Now()
	return s.repo.Create(ctx, banner)
}
