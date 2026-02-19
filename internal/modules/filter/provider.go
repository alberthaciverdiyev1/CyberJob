package filter

import (
	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/filter/delivery/http"
	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/filter/repository"
	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/filter/service"
	"go.uber.org/zap"
	"gorm.io/gorm"
)

func InitFilterModule(db *gorm.DB, logger *zap.Logger) *http.FilterHandler {
	repo := repository.NewFilterRepository(db)
	svc := service.NewFilterService(repo)
	return http.NewFilterHandler(svc, logger)
}
