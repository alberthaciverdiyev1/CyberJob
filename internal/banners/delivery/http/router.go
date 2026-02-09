package http

import (
	"github.com/go-chi/chi/v5"
	httpSwagger "github.com/swaggo/http-swagger"
)

func RegisterHandlers(r chi.Router, handler *BannerHandler) {
	r.Get("/swagger/*", httpSwagger.WrapHandler)

	r.Route("/banners", func(r chi.Router) {
		r.Post("/", handler.Create)       // POST /banners
		r.Get("/", handler.List)          // GET /banners
		r.Get("/{id}", handler.GetByID)   // GET /banners/12
		r.Put("/{id}", handler.Update)    // PUT /banners/12
		r.Delete("/{id}", handler.Delete) // DELETE /banners/12
	})
}
