package app

import (
	"net/http"

	_ "github.com/alberthaciverdiyev1/CyberJob/docs"
	customMW "github.com/alberthaciverdiyev1/CyberJob/internal/middleware"
	http2 "github.com/alberthaciverdiyev1/CyberJob/internal/modules/banners/delivery/http"
	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/banners/domain"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/config"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/db" // Paket adı 'db'
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/logger"
	"github.com/go-chi/chi/v5"
	"github.com/go-chi/chi/v5/middleware"
	"go.uber.org/zap"
)

func Run() {
	cfg := config.NewConfig()

	logger.InitLogger(cfg.LogLevel)
	defer logger.Log.Sync()

	gormDB := db.ConnectDB(cfg.DatabaseURL)

	err := gormDB.AutoMigrate(
		&domain.Banner{},
		//&domain.CompanyCategory{},
		// &jobs.Job{},
	)

	if err != nil {
		logger.Log.Fatal("Database migration failed", zap.Error(err))
	}

	r := chi.NewRouter()
	r.Use(middleware.Recoverer)
	r.Use(customMW.ZapLogger)

	bannerHdl := http2.InitBannerModule(gormDB)
	http2.RegisterHandlers(r, bannerHdl)

	logger.Log.Info("Server is starting on port " + cfg.AppPort)
	server := &http.Server{
		Addr:    ":" + cfg.AppPort,
		Handler: r,
	}

	if err := server.ListenAndServe(); err != nil {
		logger.Log.Fatal("Server failed to start", zap.Error(err))
	}
}
