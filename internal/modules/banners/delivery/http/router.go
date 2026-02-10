package http

import (
	"github.com/go-chi/chi/v5"
)

func RegisterHandlers(r chi.Router, handler *BannerHandler) {
	r.Route("/banners", func(r chi.Router) {
		r.Post("/", handler.Create)       // POST /banners
		r.Get("/", handler.List)          // GET /banners
		r.Get("/{id}", handler.GetByID)   // GET /banners/12
		r.Put("/{id}", handler.Update)    // PUT /banners/12
		r.Delete("/{id}", handler.Delete) // DELETE /banners/12
	})
}
