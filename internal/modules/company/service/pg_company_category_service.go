package service

import (
	"context"
	"errors"
	"fmt"
	"strings"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/company/domain"
)

var (
	ErrCategoryAlreadyExists = errors.New("category already exists with given name")
	ErrCategoryNameEmpty     = errors.New("category name can't be empty")
)

type companyCategoryService struct {
	repo domain.CompanyCategoryRepository
}

func NewCategoryService(repo domain.CompanyCategoryRepository) domain.CompanyCategoryService {
	return &companyCategoryService{repo: repo}
}

func (s *companyCategoryService) CreateCategory(ctx context.Context, cat *domain.CompanyCategory) error {
	cat.Name = strings.TrimSpace(cat.Name)
	if cat.Name == "" {
		return ErrCategoryNameEmpty
	}

	existing, _ := s.repo.GetByName(ctx, cat.Name)
	if existing != nil && existing.ID != 0 {
		return ErrCategoryAlreadyExists
	}

	return s.repo.Create(ctx, cat)
}

func (s *companyCategoryService) GetAllCategories(ctx context.Context) ([]domain.CompanyCategory, error) {
	return s.repo.GetAll(ctx)
}

func (s *companyCategoryService) GetCategoryByID(ctx context.Context, id uint) (*domain.CompanyCategory, error) {
	return s.repo.GetByID(ctx, id)
}

func (s *companyCategoryService) UpdateCategory(ctx context.Context, cat *domain.CompanyCategory) error {
	cat.Name = strings.TrimSpace(cat.Name)
	if cat.Name == "" {
		return ErrCategoryNameEmpty
	}

	_, err := s.repo.GetByID(ctx, cat.ID)
	if err != nil {
		return errors.New("cant find category with given id")
	}

	duplicate, _ := s.repo.GetByName(ctx, cat.Name)
	if duplicate != nil && duplicate.ID != 0 && duplicate.ID != cat.ID {
		return ErrCategoryAlreadyExists
	}

	return s.repo.Update(ctx, cat)
}

func (s *companyCategoryService) DeleteCategory(ctx context.Context, id uint) error {
	err := s.repo.Delete(ctx, id)
	if err != nil {
		return fmt.Errorf("an error occurred when delete category: %w", err)
	}
	return nil
}
