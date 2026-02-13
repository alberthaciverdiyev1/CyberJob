package service

import (
	"context"
	"errors"
	"os"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/partner/domain"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/api"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/storage"
	"gorm.io/gorm"
)

type partnerService struct {
	repo domain.PartnerRepository
}

func NewPartnerService(repo domain.PartnerRepository) PartnerService {
	return &partnerService{repo: repo}
}

func (p *partnerService) Create(ctx context.Context, req CreatePartnerRequest) error {
	var imagePath string
	var err error

	if req.Image != nil {
		imagePath, err = storage.UploadFile(req.Image, req.Ext, "partners")
		if err != nil {
			return err
		}
	}

	newPartner := domain.Partner{
		Name:  req.Name,
		Image: imagePath,
		Link:  req.Link,
	}

	return p.repo.Create(ctx, newPartner)
}

func (p *partnerService) List(ctx context.Context) ([]PartnerResponse, error) {
	partners, err := p.repo.GetAll(ctx)
	if err != nil {
		return nil, err
	}

	var response []PartnerResponse
	for _, item := range partners {
		response = append(response, PartnerResponse{
			ID:    item.ID,
			Name:  item.Name,
			Image: item.Image,
			Link:  item.Link,
		})
	}

	return response, nil
}

func (p *partnerService) Delete(ctx context.Context, id uint) error {
	_, err := p.repo.GetByID(ctx, id)
	if err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return api.NewNotFoundError("partner not found")
		}
		return err
	}

	return p.repo.Delete(ctx, id)
}

func (p *partnerService) Update(ctx context.Context, id uint, req UpdatePartnerRequest) error {
	existing, err := p.repo.GetByID(ctx, id)
	if err != nil {
		return api.NewNotFoundError("partner not found")
	}

	if req.Name != nil {
		existing.Name = *req.Name
	}

	if req.Image != nil {
		oldImage := existing.Image

		ext := ".jpg"
		if req.Ext != nil {
			ext = *req.Ext
		}

		imagePath, err := storage.UploadFile(req.Image, ext, "partners")
		if err != nil {
			return err
		}

		existing.Image = imagePath

		if oldImage != "" {
			_ = os.Remove(oldImage)
		}
	}

	if req.Link != nil {
		existing.Link = *req.Link
	}

	return p.repo.Update(ctx, *existing)
}
