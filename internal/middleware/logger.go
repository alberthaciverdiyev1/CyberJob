package middleware

import (
	"net/http"
	"time"

	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/logger"
	"go.uber.org/zap"
)

func ZapLogger(next http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		start := time.Now()
		next.ServeHTTP(w, r)

		logger.Log.Info("Inbound Request",
			zap.String("method", r.Method),
			zap.String("path", r.URL.Path),
			zap.Duration("latency", time.Since(start)),
		)
	})
}
