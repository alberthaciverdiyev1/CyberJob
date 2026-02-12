package api

import "net/http"

type AppError struct {
	StatusCode int    `json:"-"`
	ErrMsg     string `json:"error"`
	RawErr     error  `json:"-"`
}

func (e *AppError) Error() string {
	return e.ErrMsg
}
func NewNotFoundError(msg string) *AppError {
	return &AppError{ErrMsg: msg, StatusCode: http.StatusNotFound}
}
func NewBadRequestError(msg string) *AppError {
	return &AppError{ErrMsg: msg, StatusCode: http.StatusBadRequest}
}
func NewInternalError(err error) *AppError {
	return &AppError{StatusCode: http.StatusInternalServerError, ErrMsg: "Custom Internal Server Error", RawErr: err}
}
