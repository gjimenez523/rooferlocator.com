﻿using Abp.Application.Services;
using rooferlocator.com.Common.Companies.Dtos;

namespace rooferlocator.com.Common.Companies
{
    public interface ICompanyAppService : IApplicationService
    {
        CreditsHero.Company.Dtos.CompanyConfigsDto GetCompanyConfig(CreditsHero.Common.Companies.Dtos.GetCompanyInput input);
        CreditsHero.Common.Companies.Dtos.CompanyDto GetCompany(CreditsHero.Common.Companies.Dtos.GetCompanyInput input);
        GetCompaniesOutput GetCompanies(GetCompanyInput input);
        void UpdateCompany(UpdateCompanyInput input);
        void CreateCompany(CreateCompanyInput input);
    }
}
