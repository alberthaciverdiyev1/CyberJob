package service

import "context"

type PartnerService interface {
	Create(ctx context.Context, partner CreatePartnerRequest) error
	List(ctx context.Context) ([]PartnerResponse, error)
	Delete(ctx context.Context, id uint) error
	Update(ctx context.Context, partner UpdatePartnerRequest) error
}
