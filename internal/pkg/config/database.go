package config

import (
	"log"

	"github.com/alberthaciverdiyev1/CyberJob/internal/banners/domain"
	"gorm.io/driver/postgres"
	"gorm.io/gorm"
)

func ConnectDB() *gorm.DB {
	dsn := "host=localhost user=admin password=secret dbname=cyberjob_db port=5432 sslmode=disable"
	db, err := gorm.Open(postgres.Open(dsn), &gorm.Config{})
	if err != nil {
		log.Fatal("Failed to connect to database: ", err)
	}

	// Migrations
	err = db.AutoMigrate(&domain.Banner{})
	if err != nil {
		log.Fatal("Migration failed: ", err)
	}

	return db
}
