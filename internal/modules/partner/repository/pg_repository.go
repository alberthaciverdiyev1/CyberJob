package repository

import (
	"context"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/partner/domain"
	"gorm.io/gorm"
)

type partnerRepository struct {
	db *gorm.DB
}

func NewPartnerRepository(db *gorm.DB) domain.PartnerRepository {
	return &partnerRepository{db: db}
}

func (p partnerRepository) Create(ctx context.Context, partner domain.Partner) error {
	//TODO implement me
	panic("implement me")
}

func (p partnerRepository) GetAll(ctx context.Context) ([]domain.Partner, error) {
	//TODO implement me
	panic("implement me")
}

func (p partnerRepository) GetByID(ctx context.Context, id uint) (*domain.Partner, error) {
	//TODO implement me
	panic("implement me")
}

func (p partnerRepository) Update(ctx context.Context, partner domain.Partner) error {
	//TODO implement me
	panic("implement me")
}

func (p partnerRepository) Delete(ctx context.Context, id uint) error {
	//TODO implement me
	panic("implement me")
}
