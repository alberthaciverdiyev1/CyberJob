package http

import "github.com/alberthaciverdiyev1/CyberJob/internal/modules/vacancy/domain"

func ToCreateParams(req CreateVacancyRequest) domain.CreateVacancyParams {
	return domain.CreateVacancyParams{
		Name:         req.Name,
		Requirements: req.Requirements,
		Description:  req.Description,
		City:         req.City,
		BannerImage:  req.BannerImage,
		MinSalary:    req.MinSalary,
		MaxSalary:    req.MaxSalary,
		MinAge:       req.MinAge,
		MaxAge:       req.MaxAge,
		Email:        req.Email,
		CompanyID:    req.CompanyID,
		CategoryID:   req.CategoryID,
		FilterIDs:    req.FilterIDs,
		ExpireDate:   req.ExpireDate,
	}
}

func ToUpdateParams(id uint, req UpdateVacancyRequest) domain.UpdateVacancyParams {
	return domain.UpdateVacancyParams{
		ID:           id,
		Name:         req.Name,
		Requirements: req.Requirements,
		Description:  req.Description,
		City:         req.City,
		BannerImage:  req.BannerImage,
		MinSalary:    req.MinSalary,
		MaxSalary:    req.MaxSalary,
		MinAge:       req.MinAge,
		MaxAge:       req.MaxAge,
		Email:        req.Email,
		CompanyID:    req.CompanyID,
		CategoryID:   req.CategoryID,
		FilterIDs:    req.FilterIDs,
		ExpireDate:   req.ExpireDate,
	}
}
