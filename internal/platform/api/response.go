package api

import (
	"encoding/json"
	"net/http"
)

type MessageResponse struct {
	Success bool   `json:"success" example:"true"`
	Message string `json:"message" example:"Operation successful"`
}

type APIResponse[T any] struct {
	Success bool   `json:"success"`
	Message string `json:"message"`
	Data    T      `json:"data,omitempty"`
}

func SuccessResponse[T any](message string, data T) APIResponse[T] {
	return APIResponse[T]{
		Success: true,
		Message: message,
		Data:    data,
	}
}

func SuccessMessage(message string) APIResponse[any] {
	return APIResponse[any]{
		Success: true,
		Message: message,
		Data:    nil,
	}
}

func ErrorResponse(message string) APIResponse[any] {
	return APIResponse[any]{
		Success: false,
		Message: message,
		Data:    nil,
	}
}

func WriteJSON(w http.ResponseWriter, status int, response any) {
	buf, err := json.Marshal(response)
	if err != nil {
		w.Header().Set("Content-Type", "application/json")
		w.WriteHeader(http.StatusInternalServerError)
		_, _ = w.Write([]byte(`{"success":false,"message":"Internal server error"}`))
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(status)
	_, _ = w.Write(buf)
}
