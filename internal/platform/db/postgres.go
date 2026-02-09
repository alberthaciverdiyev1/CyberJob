package db

import (
	"github.com/alberthaciverdiyev1/CyberJob/internal/banners/domain"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/logger"
	"go.uber.org/zap"
	"gorm.io/driver/postgres"
	"gorm.io/gorm"
)

func ConnectDB(dsn string) *gorm.DB {
	db, err := gorm.Open(postgres.Open(dsn), &gorm.Config{})
	if err != nil {
		logger.Log.Fatal("Failed to connect to database", zap.Error(err))
	}

	err = db.AutoMigrate(
		&domain.Banner{},
		// &jobs.Job{},
	)

	if err != nil {
		logger.Log.Fatal("Database migration failed", zap.Error(err))
	}

	logger.Log.Info("Database connection established and migrations completed")
	return db
}
