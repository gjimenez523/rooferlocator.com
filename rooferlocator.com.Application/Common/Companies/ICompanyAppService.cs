﻿using Abp.Application.Services;
using rooferlocator.com.Common.Companies.Dtos;

namespace rooferlocator.com.Common.Companies
{
    public interface ICompanyAppService : IApplicationService
    {
        GetCompaniesOutput GetCompanies(GetCompanyInput input);
        void UpdateCompany(UpdateCompanyInput input);
        void CreateCompany(CreateCompanyInput input);
    }
}
