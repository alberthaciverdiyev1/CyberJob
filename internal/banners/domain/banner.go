package domain

import (
	"context"
	"time"
)

type Banner struct {
	BaseEntity
	ImageUrl       string    `json:"image_url"`
	Type           string    `json:"type"`
	Page           string    `json:"page"`
	ExpirationDate time.Time `json:"expiration_date"`
}
type BannerRepository interface {
	GetByID(ctx context.Context, id uint) (*Banner, error)
	Create(ctx context.Context, banner *Banner) error
	GetAll(ctx context.Context) ([]Banner, error)
	Delete(ctx context.Context, id uint) error
	Update(ctx context.Context, banner *Banner) error
}

type BannerService interface {
	GetActiveBanners(ctx context.Context) ([]Banner, error)

	CreateBanner(ctx context.Context, banner *Banner) error
	UpdateBanner(ctx context.Context, banner *Banner) error
	DeleteBanner(ctx context.Context, id uint) error
	GetBannerDetail(ctx context.Context, id uint) (*Banner, error)
}
