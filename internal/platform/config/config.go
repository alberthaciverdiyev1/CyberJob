package config

import (
	"os"
)

type Config struct {
	AppPort     string
	DatabaseURL string
	LogLevel    string // debug, info, error
}

func NewConfig() *Config {
	return &Config{
		AppPort:     getEnv("APP_PORT", "8080"),
		DatabaseURL: getEnv("DATABASE_URL", "host=localhost user=admin password=secret dbname=cyberjob_db port=5432 sslmode=disable"),
		LogLevel:    getEnv("LOG_LEVEL", "debug"),
	}
}

func getEnv(key, fallback string) string {
	if value, ok := os.LookupEnv(key); ok {
		return value
	}
	return fallback
}
