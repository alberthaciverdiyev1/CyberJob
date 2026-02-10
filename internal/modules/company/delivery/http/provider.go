package http

import (
	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/company/repository"
	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/company/service"
	"gorm.io/gorm"
)

func InitCompanyCategoryModule(db *gorm.DB) *CompanyCategoryHandler {
	repo := repository.NewCompanyCategoryRepository(db)
	svc := service.NewCategoryService(repo)
	return NewCompanyCategoryHandler(svc)
}
