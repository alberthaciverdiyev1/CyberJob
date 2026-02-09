package service

import (
	"context"
	"errors"
	"fmt"
	"strings"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/company/domain"
)

var (
	ErrCategoryAlreadyExists = errors.New("Category Already Exists With Given Name")
	ErrCategoryNameEmpty     = errors.New("Category Name Can't be Empty")
)

type categoryService struct {
	repo domain.CategoryRepository
}

func NewCategoryService(repo domain.CategoryRepository) domain.CategoryService {
	return &categoryService{repo: repo}
}

func (s *categoryService) CreateCategory(ctx context.Context, cat *domain.CompanyCategory) error {
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

func (s *categoryService) GetAllCategories(ctx context.Context) ([]domain.CompanyCategory, error) {
	return s.repo.GetAll(ctx)
}

func (s *categoryService) GetCategoryByID(ctx context.Context, id uint) (*domain.CompanyCategory, error) {
	return s.repo.GetByID(ctx, id)
}

func (s *categoryService) UpdateCategory(ctx context.Context, cat *domain.CompanyCategory) error {
	cat.Name = strings.TrimSpace(cat.Name)
	if cat.Name == "" {
		return ErrCategoryNameEmpty
	}

	_, err := s.repo.GetByID(ctx, cat.ID)
	if err != nil {
		return errors.New("Cant find category with given id")
	}

	duplicate, _ := s.repo.GetByName(ctx, cat.Name)
	if duplicate != nil && duplicate.ID != 0 && duplicate.ID != cat.ID {
		return ErrCategoryAlreadyExists
	}

	return s.repo.Update(ctx, cat)
}

func (s *categoryService) DeleteCategory(ctx context.Context, id uint) error {
	err := s.repo.Delete(ctx, id)
	if err != nil {
		return fmt.Errorf("An Error Occuren When Delete Category: %w", err)
	}
	return nil
}
