package domain

import "errors"

var (
	ErrFilterNotFound    = errors.New("filter not found")
	ErrInvalidFilterID   = errors.New("invalid filter id")
	ErrDuplicateKey      = errors.New("filter key already exists")
	ErrFilterKeyRequired = errors.New("filter key is required")
)
