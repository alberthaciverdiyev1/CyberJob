package app

import (
	"net/http"

	_ "github.com/alberthaciverdiyev1/CyberJob/docs"
	bannerHandler "github.com/alberthaciverdiyev1/CyberJob/internal/banners/delivery/http"
	customMW "github.com/alberthaciverdiyev1/CyberJob/internal/middleware"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/config"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/db"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/logger"
	"github.com/go-chi/chi/v5"
	"github.com/go-chi/chi/v5/middleware"
	"go.uber.org/zap"
)

func Run() {
	cfg := config.NewConfig()
	// Logger & DB
	logger.InitLogger(cfg.LogLevel)
	defer logger.Log.Sync()
	db := db.ConnectDB(cfg.DatabaseURL)

	// Router Setup
	r := chi.NewRouter()
	r.Use(middleware.Recoverer)
	r.Use(customMW.ZapLogger)

	// Modules
	bannerHdl := bannerHandler.InitBannerModule(db)
	bannerHandler.RegisterHandlers(r, bannerHdl)

	// Server
	logger.Log.Info("Server is starting on port 8080")
	if err := http.ListenAndServe(":"+cfg.AppPort, r); err != nil {
		logger.Log.Fatal("Server failed to start", zap.Error(err))
	}
}
