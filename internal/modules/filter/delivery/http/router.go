package http

import "github.com/go-chi/chi/v5"

func RegisterHandlers(r chi.Router, handler *FilterHandler) {
	r.Route("/filters", func(r chi.Router) {
		r.Get("/", handler.GetAll)
		r.Post("/", handler.Create)
		r.Get("/{id}", handler.GetByID)
		r.Delete("/{id}", handler.Delete)
		r.Put("/{id}", handler.Update)
	})
}
