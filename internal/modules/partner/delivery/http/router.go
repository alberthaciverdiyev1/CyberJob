package http

import "github.com/go-chi/chi/v5"

func RegisterHandlers(r chi.Router, handler *PartnerHandler) {
	r.Route("/partners", func(r chi.Router) {
		r.Get("/", handler.List)
		r.Post("/", handler.Create)
		r.Delete("/{id}", handler.Delete)
		r.Put("/{id}", handler.Update)
	})
}
