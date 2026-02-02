using AutoMapper;
using CyberJob.Core.DTOs.BannerDto;
using CyberJob.Core.DTOs.Category;
using CyberJob.Core.DTOs.Company;
using CyberJob.Core.DTOs.CompanyCategory;
using CyberJob.Core.DTOs.Partner;
using CyberJob.Core.Entities;

namespace CyberJob.Service.Mapping;

public class MapProfile : Profile
{
    public MapProfile()
    {   //Banners
        CreateMap<Banner, BannerResponse>().ReverseMap();
        CreateMap<BannerCreateRequest, Banner>();
        CreateMap<BannerUpdateRequest, Banner>();
        
        //Categories
        CreateMap<Category,CategoryResponse>().ReverseMap();
        CreateMap<CreateCategoryRequest, Category>();
        CreateMap<UpdateCategoryRequest, Category>();
        
        //Partners
        CreateMap<Partner,PartnerResponse>().ReverseMap();
        CreateMap<CreatePartnerRequest, Partner>();
        CreateMap<UpdatePartnerRequest, Partner>();
        
        //Companies
        CreateMap<Company,CompanyResponse>().ReverseMap();
        CreateMap<CreateCompanyRequest, Company>();
        CreateMap<UpdateCompanyRequest, Company>();
        
        //Company Categories
        CreateMap<CompanyCategory, CompanyCategoryResponse>().ReverseMap();
        CreateMap<CreateCompanyCategoryRequest, CompanyCategory>();
        CreateMap<UpdateCompanyCategoryRequest, CompanyCategory>();
    }
}