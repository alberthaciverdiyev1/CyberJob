package repository

import (
	"context"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/faq/domain"
	"gorm.io/gorm"
)

type faqRepository struct {
	db *gorm.DB
}

func NewFAQRepository(db *gorm.DB) domain.FAQRepository {
	return faqRepository{db: db}
}

func (f faqRepository) GetAll(ctx context.Context) ([]domain.FAQ, error) {

	var faqs []domain.FAQ
	err := f.db.WithContext(ctx).Find(&faqs).Error
	return faqs, err

}

func (f faqRepository) GetByID(ctx context.Context, id uint) (*domain.FAQ, error) {
	var faq domain.FAQ
	err := f.db.WithContext(ctx).First(&faq, id).Error
	return &faq, err
}

func (f faqRepository) Create(ctx context.Context, faq *domain.FAQ) error {

	return f.db.WithContext(ctx).Create(faq).Error
}

func (f faqRepository) Update(ctx context.Context, faq *domain.FAQ) error {
	return f.db.WithContext(ctx).Save(faq).Error
}

func (f faqRepository) Delete(ctx context.Context, id uint) error {

	return f.db.WithContext(ctx).Delete(&domain.FAQ{}, id).Error
}
