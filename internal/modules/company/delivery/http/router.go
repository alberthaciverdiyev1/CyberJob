package http

import (
	"github.com/go-chi/chi/v5"
)

func RegisterHandlers(r chi.Router, catHandler *CompanyCategoryHandler, compHandler *CompanyHandler) {

	r.Route("/company-categories", func(r chi.Router) {
		r.Post("/", catHandler.Create)             // POST /company-categories
		r.Get("/", catHandler.List)                // GET /company-categories
		r.Get("/{id}", catHandler.GetCategoryByID) // GET /company-categories/{id}
		r.Put("/{id}", catHandler.Update)          // PUT /company-categories/{id}
		r.Delete("/{id}", catHandler.Delete)       // DELETE /company-categories/{id}
	})

	r.Route("/companies", func(r chi.Router) {
		r.Post("/", compHandler.Register)     // POST /companies
		r.Get("/", compHandler.List)          // GET /companies
		r.Get("/{id}", compHandler.GetByID)   // GET /companies/{id}
		r.Put("/{id}", compHandler.Update)    // PUT /companies/{id}
		r.Delete("/{id}", compHandler.Delete) // DELETE /companies/{id}
	})
}
