package repository

import (
	"context"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/category/domain"
	"gorm.io/gorm"
)

type categoryRepository struct {
	db *gorm.DB
}

func NewCategoryRepository(db *gorm.DB) domain.CategoryRepository {
	return &categoryRepository{db: db}
}

func (c categoryRepository) Create(ctx context.Context, cat *domain.Category) error {
	return c.db.WithContext(ctx).Create(cat).Error
}

func (c categoryRepository) GetAllWithChildren(ctx context.Context) ([]domain.Category, error) {
	var cats []domain.Category

	err := c.db.WithContext(ctx).
		Preload("Children").
		Where("parent_id IS NULL OR parent_id = 0").
		Find(&cats).Error

	return cats, err
}

func (c categoryRepository) GetAll(ctx context.Context) ([]domain.Category, error) {
	var cats []domain.Category

	err := c.db.WithContext(ctx).
		Where("parent_id IS NULL OR parent_id = 0").
		Find(&cats).Error

	return cats, err
}

func (c categoryRepository) GetByID(ctx context.Context, id uint) (*domain.Category, error) {
	var cat domain.Category
	err := c.db.WithContext(ctx).
		Preload("Children").
		Where("parent_id IS NULL OR parent_id = 0").First(&cat, id).Error
	if err != nil {
		return nil, err
	}
	return &cat, nil
}

func (c categoryRepository) Update(ctx context.Context, cat *domain.Category) error {
	return c.db.WithContext(ctx).Save(cat).Error
}

func (c categoryRepository) Delete(ctx context.Context, id uint) error {
	return c.db.WithContext(ctx).Delete(&domain.Category{}, id).Error
}

func (c categoryRepository) GetByName(ctx context.Context, name string) (*domain.Category, error) {
	var cat domain.Category

	err := c.db.WithContext(ctx).Where("name = ?", name).First(&cat).Error

	if err != nil {
		return nil, err
	}

	return &cat, nil
}
