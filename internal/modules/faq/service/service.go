package service

import (
	"context"
	"errors"

	"github.com/alberthaciverdiyev1/CyberJob/internal/modules/faq/domain"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/api"
	"github.com/alberthaciverdiyev1/CyberJob/internal/platform/validation"
	"github.com/go-playground/validator/v10"
	"gorm.io/gorm"
)

var validate = validator.New()

type faqService struct {
	repo domain.FAQRepository
}

func NewFAQService(repo domain.FAQRepository) FAQService {
	return &faqService{repo: repo}
}

func (s *faqService) GetAll(ctx context.Context) ([]FAQResponse, error) {
	faqs, err := s.repo.GetAll(ctx)
	if err != nil {
		return nil, err
	}

	var response []FAQResponse
	for _, f := range faqs {
		response = append(response, FAQResponse{
			ID:         f.ID,
			QuestionAz: f.QuestionAz,
			AnswerAz:   f.AnswerAz,
			QuestionEn: f.QuestionEn,
			AnswerEn:   f.AnswerEn,
			QuestionRu: f.QuestionRu,
			AnswerRu:   f.AnswerRu,
			CreatedAt:  f.CreatedAt.Format("02.01.2006 15:04"),
		})
	}
	return response, nil
}

func (s *faqService) GetByID(ctx context.Context, id uint) (*FAQResponse, error) {
	f, err := s.repo.GetByID(ctx, id)
	if err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, api.NewNotFoundError("faq not found")
		}
		return nil, err
	}

	return &FAQResponse{
		ID:         f.ID,
		QuestionAz: f.QuestionAz,
		AnswerAz:   f.AnswerAz,
		QuestionEn: f.QuestionEn,
		AnswerEn:   f.AnswerEn,
		QuestionRu: f.QuestionRu,
		AnswerRu:   f.AnswerRu,
		CreatedAt:  f.CreatedAt.Format("02.01.2006 15:04"),
	}, nil
}

func (s *faqService) Create(ctx context.Context, req CreateFAQRequest) error {

	if errMsg := validation.ValidateStruct(req); errMsg != "" {
		return &api.AppError{
			StatusCode: 400,
			ErrMsg:     errMsg,
			RawErr:     errors.New(errMsg),
		}
	}

	faq := &domain.FAQ{
		QuestionAz: req.QuestionAz,
		AnswerAz:   req.AnswerAz,
		QuestionEn: req.QuestionEn,
		AnswerEn:   req.AnswerEn,
		QuestionRu: req.QuestionRu,
		AnswerRu:   req.AnswerRu,
	}
	return s.repo.Create(ctx, faq)
}

func (s *faqService) Update(ctx context.Context, req UpdateFAQRequest) error {
	existing, err := s.repo.GetByID(ctx, req.ID)
	if err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return api.NewNotFoundError("faq not found")
		}
		return err
	}

	if req.QuestionAz != "" {
		existing.QuestionAz = req.QuestionAz
	}
	if req.AnswerAz != "" {
		existing.AnswerAz = req.AnswerAz
	}
	if req.QuestionEn != "" {
		existing.QuestionEn = req.QuestionEn
	}
	if req.AnswerEn != "" {
		existing.AnswerEn = req.AnswerEn
	}
	if req.QuestionRu != "" {
		existing.QuestionRu = req.QuestionRu
	}
	if req.AnswerRu != "" {
		existing.AnswerRu = req.AnswerRu
	}

	return s.repo.Update(ctx, existing)
}

func (s *faqService) Delete(ctx context.Context, id uint) error {
	_, err := s.repo.GetByID(ctx, id)
	if err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return api.NewNotFoundError("faq not found")
		}
		return err
	}

	return s.repo.Delete(ctx, id)
}
