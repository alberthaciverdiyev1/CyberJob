package http

import (
	"encoding/json"
	"net/http"
)

type APIResponse[T any] struct {
	Success bool   `json:"success"`
	Message string `json:"message"`
	Data    T      `json:"data,omitempty" swaggertype:"object"`
}

func SuccessResponse(message string, data interface{}) APIResponse[interface{}] {
	return APIResponse[interface{}]{
		Success: true,
		Message: message,
		Data:    data,
	}
}

func ErrorResponse(message string) APIResponse[interface{}] {
	return APIResponse[interface{}]{
		Success: false,
		Message: message,
		Data:    nil,
	}
}

func WriteJSON(w http.ResponseWriter, status int, response interface{}) {
	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(status)
	json.NewEncoder(w).Encode(response)
}
