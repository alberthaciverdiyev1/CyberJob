package app

import (
	"net/http"

	_ "github.com/alberthaciverdiyev1/CyberJob/docs"
	customMW "github.com/alberthaciverdiyev1/CyberJob/internal/middleware"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/faq"
	faqHttp "github.com/alberthaciverdiyev1/CyberJob/internal/modules/faq/delivery/http"
	faqDomain "github.com/alberthaciverdiyev1/CyberJob/internal/modules/faq/domain"

	bannerHttp "github.com/alberthaciverdiyev1/CyberJob/internal/modules/banners/delivery/http"
	bannerDomain "github.com/alberthaciverdiyev1/CyberJob/internal/modules/banners/domain"
	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/partner"

	partnerHttp "github.com/alberthaciverdiyev1/CyberJob/internal/modules/partner/delivery/http"
	partnerDomain "github.com/alberthaciverdiyev1/CyberJob/internal/modules/partner/domain"

	categoryHttp "github.com/alberthaciverdiyev1/CyberJob/internal/modules/category/delivery/http"
	categoryDomain "github.com/alberthaciverdiyev1/CyberJob/internal/modules/category/domain"
	companyHttp "github.com/alberthaciverdiyev1/CyberJob/internal/modules/company/delivery/http"
	companyDomain "github.com/alberthaciverdiyev1/CyberJob/internal/modules/company/domain"

	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/config"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/db"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/logger"

	"github.com/go-chi/chi/v5"
	"github.com/go-chi/chi/v5/middleware"
	"github.com/swaggo/http-swagger"
	"go.uber.org/zap"
)

func Run() {
	cfg := config.NewConfig()
	logger.InitLogger(cfg.LogLevel)
	defer logger.Log.Sync()

	gormDB := db.ConnectDB(cfg.DatabaseURL)

	// Migration
	err := gormDB.AutoMigrate(
		&bannerDomain.Banner{},
		&companyDomain.CompanyCategory{},
		&companyDomain.Company{},
		&categoryDomain.Category{},
		&partnerDomain.Partner{},
		&faqDomain.FAQ{},
	)
	if err != nil {
		logger.Log.Fatal("Database migration failed", zap.Error(err))
	}

	r := chi.NewRouter()
	r.Use(middleware.Recoverer)
	r.Use(customMW.ZapLogger)

	r.Get("/swagger/*", httpSwagger.WrapHandler)
	r.Handle("/public/*", http.StripPrefix("/public/", http.FileServer(http.Dir("./public"))))
	// Banner
	bannerHdl := bannerHttp.InitBannerModule(gormDB)
	bannerHttp.RegisterHandlers(r, bannerHdl)

	// Company
	companyCategoryHdl := companyHttp.InitCompanyCategoryModule(gormDB)
	companyHdl := companyHttp.InitCompanyModule(gormDB)
	companyHttp.RegisterHandlers(r, companyCategoryHdl, companyHdl)

	// Category
	categoryHdl := categoryHttp.InitCategoryModule(gormDB)
	categoryHttp.RegisterHandlers(r, categoryHdl)
	//Partner
	partnerHdl := partner.InitPartnerModule(gormDB)
	partnerHttp.RegisterHandlers(r, partnerHdl)

	//FAQ
	faqHdl := faq.InitFaqModule(gormDB)
	faqHttp.RegisterHandlers(r, faqHdl)

	addr := ":" + cfg.AppPort
	logger.Log.Info("Server is starting", zap.String("port", cfg.AppPort))
	logger.Log.Info("Swagger docs available on http://localhost:" + cfg.AppPort + "/swagger/index.html")

	server := &http.Server{
		Addr:    addr,
		Handler: r,
	}

	if err := server.ListenAndServe(); err != nil {
		logger.Log.Fatal("Server failed to start", zap.Error(err))
	}
}
