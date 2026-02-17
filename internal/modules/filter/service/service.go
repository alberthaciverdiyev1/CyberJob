package service

import (
	"context"
	"errors"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/filter/domain"
	"gorm.io/gorm"
)

type filterService struct {
	repo domain.FilterRepository
}

func NewFilterService(repo domain.FilterRepository) domain.FilterService {
	return &filterService{repo: repo}
}

func (s *filterService) GetAll(ctx context.Context) ([]domain.Filter, error) {
	return s.repo.GetAll(ctx)
}

func (s *filterService) GetByID(ctx context.Context, id uint) (*domain.Filter, error) {
	if id == 0 {
		return nil, domain.ErrInvalidFilterID
	}

	filter, err := s.repo.GetByID(ctx, id)

	if err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, domain.ErrFilterNotFound
		}
		return nil, err
	}

	return filter, nil
}

func (s *filterService) Create(ctx context.Context, params domain.CreateFilterParams) error {
	filter := &domain.Filter{
		Key:    params.Key,
		NameAz: params.NameAz,
		NameEn: params.NameEn,
		NameRu: params.NameRu,
	}

	err := s.repo.Create(ctx, filter)
	if err != nil {
		return err
	}
	return nil
}

func (s *filterService) Update(ctx context.Context, params domain.UpdateFilterParams) error {
	if params.ID == 0 {
		return domain.ErrInvalidFilterID
	}

	existing, err := s.repo.GetByID(ctx, params.ID)
	if err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return domain.ErrFilterNotFound
		}
		return err
	}

	hasChanges := false

	if params.Key != "" && params.Key != existing.Key {
		existing.Key = params.Key
		hasChanges = true
	}
	if params.NameAz != "" && params.NameAz != existing.NameAz {
		existing.NameAz = params.NameAz
		hasChanges = true
	}
	if params.NameEn != "" && params.NameEn != existing.NameEn {
		existing.NameEn = params.NameEn
		hasChanges = true
	}
	if params.NameRu != "" && params.NameRu != existing.NameRu {
		existing.NameRu = params.NameRu
		hasChanges = true
	}

	if !hasChanges {
		return nil
	}

	err = s.repo.Update(ctx, existing)
	if err != nil {
		return err
	}

	return nil
}

func (s *filterService) Delete(ctx context.Context, id uint) error {
	if id == 0 {
		return domain.ErrInvalidFilterID
	}

	_, err := s.repo.GetByID(ctx, id)
	if err != nil {
		return domain.ErrFilterNotFound
	}

	return s.repo.Delete(ctx, id)
}
