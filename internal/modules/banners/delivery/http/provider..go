package http

import (
	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/banners/repository"
	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/banners/service"
	"gorm.io/gorm"
)

func InitBannerModule(db *gorm.DB) *BannerHandler {
	repo := repository.NewBannerRepository(db)
	svc := service.NewBannerService(repo)
	return NewBannerHandler(svc)
}
