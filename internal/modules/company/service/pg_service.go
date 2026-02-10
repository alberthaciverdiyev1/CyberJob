package service

import (
	"context"
	"errors"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/company/domain"
)

var (
	ErrCompanyNotFound      = errors.New("company not found")
	ErrCompanyAlreadyExists = errors.New("a company with this name already exists")
)

type companyService struct {
	repo domain.CompanyRepository
}

func NewCompanyService(r domain.CompanyRepository) domain.CompanyService {
	return &companyService{
		repo: r,
	}
}

func (s *companyService) Register(ctx context.Context, comp *domain.Company) error {
	filter := domain.CompanyFilter{Name: comp.Name, Limit: 1}
	existing, err := s.repo.Filter(ctx, filter)
	if err == nil && len(existing) > 0 {
		return ErrCompanyAlreadyExists
	}

	return s.repo.Create(ctx, comp)
}

func (s *companyService) List(ctx context.Context, filter domain.CompanyFilter) ([]domain.Company, error) {
	return s.repo.Filter(ctx, filter)
}

func (s *companyService) Details(ctx context.Context, id uint) (*domain.Company, error) {
	comp, err := s.repo.Details(ctx, id)
	if err != nil {
		return nil, err
	}

	if comp == nil {
		return nil, ErrCompanyNotFound
	}

	return comp, nil
}

func (s *companyService) Update(ctx context.Context, comp *domain.Company) error {
	existing, err := s.repo.Details(ctx, comp.ID)
	if err != nil {
		return err
	}
	if existing == nil {
		return ErrCompanyNotFound
	}

	return s.repo.Update(ctx, comp)
}

func (s *companyService) Delete(ctx context.Context, id uint) error {
	return s.repo.Delete(ctx, id)
}
