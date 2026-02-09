package service

import (
	"context"
	"errors"
	"strings"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/company/domain"
)

var (
	ErrCompanyAlreadyExists = errors.New("company Already Exists With Given Name")
	ErrCompanyNameEmpty     = errors.New("company Name Can't be Empty")
)

type companyService struct {
	repo domain.CompanyRepository
}

func NewCompanyService(repo domain.CompanyRepository) domain.CompanyService {
	return &companyService{repo: repo}
}

func (c companyService) RegisterCompany(ctx context.Context, comp *domain.Company) error {
	comp.Name = strings.TrimSpace(comp.Name)
	if comp.Name == "" {
		return ErrCompanyNameEmpty
	}
	existCompany, _ := c.repo.GetByName(ctx, comp.Name)
	if existCompany != nil {
		return ErrCompanyAlreadyExists
	}
	return c.repo.Create(ctx, comp)
}

func (c companyService) GetAllCompanies(ctx context.Context) ([]domain.Company, error) {

	return c.repo.GetAll(ctx)
}

func (c companyService) GetCompanyByID(ctx context.Context, id uint) (*domain.Company, error) {
	return c.repo.GetByID(ctx, id)
}

func (c companyService) UpdateCompany(ctx context.Context, comp *domain.Company) error {
	existCompany, _ := c.repo.GetByName(ctx, comp.Name)
	if existCompany != nil && existCompany.ID != comp.ID {
		return ErrCompanyAlreadyExists
	}
	return c.repo.Update(ctx, comp)
}

func (c companyService) DeleteCompany(ctx context.Context, id uint) error {

	return c.repo.Delete(ctx, id)
}
