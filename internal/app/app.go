package app

import (
	"net/http"

	_ "github.com/alberthaciverdiyev1/CyberJob/docs"
	customMW "github.com/alberthaciverdiyev1/CyberJob/internal/middleware"
	bannerHttp "github.com/alberthaciverdiyev1/CyberJob/internal/modules/banners/delivery/http"
	bannerDomain "github.com/alberthaciverdiyev1/CyberJob/internal/modules/banners/domain"

	categoryHttp "github.com/alberthaciverdiyev1/CyberJob/internal/modules/category/delivery/http"
	categoryDomain "github.com/alberthaciverdiyev1/CyberJob/internal/modules/category/domain"

	companyHttp "github.com/alberthaciverdiyev1/CyberJob/internal/modules/company/delivery/http"
	companyDomain "github.com/alberthaciverdiyev1/CyberJob/internal/modules/company/domain"

	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/config"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/db"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/logger"

	"github.com/go-chi/chi/v5"
	"github.com/go-chi/chi/v5/middleware"
	"go.uber.org/zap"

	"github.com/swaggo/http-swagger"
)

func Run() {
	cfg := config.NewConfig()
	logger.InitLogger(cfg.LogLevel)
	defer logger.Log.Sync()

	gormDB := db.ConnectDB(cfg.DatabaseURL)
	err := gormDB.AutoMigrate(
		&bannerDomain.Banner{},
		&companyDomain.CompanyCategory{},
		&companyDomain.Company{},
		&categoryDomain.Category{},
	)
	if err != nil {
		logger.Log.Fatal("Database migration failed", zap.Error(err))
	}

	r := chi.NewRouter()

	r.Use(middleware.Recoverer)
	r.Use(customMW.ZapLogger)

	r.Get("/swagger/*", httpSwagger.WrapHandler)

	bannerHdl := bannerHttp.InitBannerModule(gormDB)
	bannerHttp.RegisterHandlers(r, bannerHdl)

	companyCategoryHdl := companyHttp.InitCompanyCategoryModule(gormDB)
	companyHdl := companyHttp.InitCompanyModule(gormDB)

	companyHttp.RegisterHandlers(r, companyCategoryHdl, companyHdl)

	categoryHdl := categoryHttp.InitCategoryModule(gormDB)
	categoryHttp.RegisterHandlers(r, categoryHdl)

	logger.Log.Info("Server is starting on port " + cfg.AppPort)
	logger.Log.Info("Swagger docs available on http://localhost:" + cfg.AppPort + "/swagger/index.html")

	server := &http.Server{
		Addr:    ":" + cfg.AppPort,
		Handler: r,
	}

	if err := server.ListenAndServe(); err != nil {
		logger.Log.Fatal("Server failed to start", zap.Error(err))
	}
}
