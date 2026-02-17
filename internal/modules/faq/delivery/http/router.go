package http

import "github.com/go-chi/chi/v5"

func RegisterHandlers(r chi.Router, h *FaqHandler) {
	r.Route("/faqs", func(r chi.Router) {
		r.Get("/", h.GetAll)
		r.Post("/", h.Create)
		r.Get("/{id}", h.GetByID)
		r.Put("/{id}", h.Update)
		r.Delete("/{id}", h.Delete)
	})
}
