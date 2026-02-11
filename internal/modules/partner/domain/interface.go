package domain

import (
	"context"
)

type PartnerRepository interface {
	Create(ctx context.Context, partner Partner) error
	GetAll(ctx context.Context) ([]Partner, error)
	GetByID(ctx context.Context, id uint) (*Partner, error)
	Update(ctx context.Context, partner Partner) error
	Delete(ctx context.Context, id uint) error
}
