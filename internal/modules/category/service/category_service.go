package service

import (
	"context"
	"errors"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/category/domain"
	"gorm.io/gorm"
)

type categoryService struct {
	repo domain.CategoryRepository
}

func NewCategoryService(repo domain.CategoryRepository) domain.CategoryService {
	return &categoryService{repo: repo}
}

func (s *categoryService) Create(ctx context.Context, req domain.CreateCategoryRequest) error {
	existingCat, err := s.repo.GetByName(ctx, req.Name)
	if err != nil && !errors.Is(err, gorm.ErrRecordNotFound) {
		return err
	}
	if existingCat != nil {
		return errors.New("category with same name already exists")
	}

	newCat := &domain.Category{
		Name:     req.Name,
		Icon:     req.Icon,
		ParentID: req.ParentID,
	}

	return s.repo.Create(ctx, newCat)
}

func (s *categoryService) GetAll(ctx context.Context) ([]domain.CategoryResponse, error) {
	categories, err := s.repo.GetAll(ctx)
	if err != nil {
		return nil, err
	}

	var res []domain.CategoryResponse
	for _, c := range categories {
		res = append(res, s.mapToResponse(c))
	}
	return res, nil
}

func (s *categoryService) GetAllWithChildren(ctx context.Context) ([]domain.CategoryResponse, error) {
	categories, err := s.repo.GetAllWithChildren(ctx)
	if err != nil {
		return nil, err
	}

	var response []domain.CategoryResponse
	for _, cat := range categories {
		response = append(response, s.mapToResponse(cat))
	}

	return response, nil
}

func (s *categoryService) GetByID(ctx context.Context, id uint) (*domain.CategoryResponse, error) {
	if id == 0 {
		return nil, errors.New("invalid category id")
	}

	cat, err := s.repo.GetByID(ctx, id)
	if err != nil {
		return nil, err
	}

	res := s.mapToResponse(*cat)
	return &res, nil
}

func (s *categoryService) Update(ctx context.Context, req domain.UpdateCategoryRequest) error {
	if req.ID == 0 {
		return errors.New("id is required to update a category")
	}

	existingCat, err := s.repo.GetByID(ctx, req.ID)
	if err != nil {
		return errors.New("category not found")
	}

	if req.Name != "" {
		existingCat.Name = req.Name
	}
	if req.Icon != "" {
		existingCat.Icon = req.Icon
	}

	existingCat.ParentID = req.ParentID

	return s.repo.Update(ctx, existingCat)
}

func (s *categoryService) Delete(ctx context.Context, id uint) error {
	if id == 0 {
		return errors.New("invalid id for deletion")
	}
	return s.repo.Delete(ctx, id)
}

func (s *categoryService) mapToResponse(c domain.Category) domain.CategoryResponse {
	var childrenRes []domain.CategoryResponse

	for _, child := range c.Children {
		childrenRes = append(childrenRes, s.mapToResponse(child))
	}

	return domain.CategoryResponse{
		ID:       c.ID,
		Name:     c.Name,
		Icon:     c.Icon,
		ParentID: c.ParentID,
		Children: childrenRes,
	}
}
