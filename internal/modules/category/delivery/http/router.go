package http

import (
	"github.com/go-chi/chi/v5"
)

func RegisterHandlers(r chi.Router, handler *CategoryHandler) {

	r.Route("/categories", func(r chi.Router) {
		r.Post("/", handler.Create)
		r.Get("/", handler.List)
		r.Get("/simple", handler.ListSimple)
		r.Get("/{id}", handler.GetByID)
		r.Put("/{id}", handler.Update)
		r.Delete("/{id}", handler.Delete)
	})
}
