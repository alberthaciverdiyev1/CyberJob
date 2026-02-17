package faq

import (
	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/faq/delivery/http"
	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/faq/repository"
	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/faq/service"
	"gorm.io/gorm"
)

func InitFaqModule(db *gorm.DB) *http.FaqHandler {

	repo := repository.NewFAQRepository(db)
	svc := service.NewFAQService(repo)

	return http.NewFaqHandler(svc)
}
