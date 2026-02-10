package http

import (
	"github.com/go-chi/chi/v5"
	httpSwagger "github.com/swaggo/http-swagger"
)

func RegisterHandlers(r chi.Router, handler *CompanyCategoryHandler) {
	r.Get("/swagger/*", httpSwagger.WrapHandler)

	r.Route("/company-categories", func(r chi.Router) {
		r.Post("/", handler.Create)             // POST /company-categories
		r.Get("/", handler.List)                // GET /company-categories
		r.Get("/{id}", handler.GetCategoryByID) // GET /company-categories/12
		r.Put("/{id}", handler.Update)          // PUT /company-categories/12
		r.Delete("/{id}", handler.Delete)       // DELETE /company-categories/12
	})
}
