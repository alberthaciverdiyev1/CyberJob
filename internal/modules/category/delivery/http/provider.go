package http

import (
	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/category/repository"
	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/category/service"
	"gorm.io/gorm"
)

func InitCategoryModule(db *gorm.DB) *CategoryHandler {
	repo := repository.NewCategoryRepository(db)
	svc := service.NewCategoryService(repo)
	return NewCategoryHandler(svc)
}
