package domain

import "context"

type FAQRepository interface {
	GetAll(ctx context.Context) ([]FAQ, error)
	GetByID(ctx context.Context, id uint) (*FAQ, error)
	Create(ctx context.Context, faq *FAQ) error
	Update(ctx context.Context, faq *FAQ) error
	Delete(ctx context.Context, id uint) error
}
