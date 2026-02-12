package partner

import (
	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/partner/delivery/http"
	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/partner/repository"
	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/partner/service"
	"gorm.io/gorm"
)

func InitPartnerModule(db *gorm.DB) *http.PartnerHandler {
	repo := repository.NewPartnerRepository(db)
	svc := service.NewPartnerService(repo)

	return http.NewPartnerHandler(svc)
}
