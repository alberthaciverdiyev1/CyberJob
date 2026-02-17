package service

import "context"

type FAQService interface {
	GetAll(ctx context.Context) ([]FAQResponse, error)
	GetByID(ctx context.Context, id uint) (*FAQResponse, error)
	Create(ctx context.Context, request CreateFAQRequest) error
	Update(ctx context.Context, request UpdateFAQRequest) error
	Delete(ctx context.Context, id uint) error
}
