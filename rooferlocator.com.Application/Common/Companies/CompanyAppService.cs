using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using AutoMapper;
using rooferlocator.com.Common.Companies;

namespace rooferlocator.com.Common.Companies
{
    public class CompanyAppService : ApplicationService, ICompanyAppService
    {
        //These members set in constructor using constructor injection.    
        private readonly ICompanyRepository _companyRepository;
        
        /// <summary>
        ///In constructor, we can get needed classes/interfaces.
        ///They are sent here by dependency injection system automatically.
        /// </summary>
        public CompanyAppService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public Dtos.GetCompaniesOutput GetCompanies(Dtos.GetCompanyInput input)
        {
            //Called specific GetAllWithPeople method of task repository.
            var companies = _companyRepository.GetAll();

            //Used AutoMapper to automatically convert List<Task> to List<TaskDto>.
            //We must declare mappings to be able to use AutoMapper
            return new Dtos.GetCompaniesOutput
            {
                Companies = Mapper.Map<List<Dtos.CompanyDto>>(companies)
            };
        }
        
        public void UpdateCompany(Dtos.UpdateCompanyInput input)
        {
            //We can use Logger, it's defined in ApplicationService base class.
            //ERROR: Logger doesn't exist in ApplicationService base Logger.Info("Updating a task for input: " + input);

            //Retrieving a task entity with given id using standard Get method of repositories.
            var company = _companyRepository.Get(input.Id.Value);

            //Updating changed properties of the retrieved task entity.
            //if (input.FirstName != string.Empty) customer.FirstName = input.FirstName.Value;
            company.Name = input.Name;
                //customer.Id = _productRepository.Load(input.AssignedPersonId.Value);
                //customer.Purchases = _productRepository.Load(input.CustomerId.Value);
        
            //We even do not call Update method of the repository.
            //Because an application service method is a 'unit of work' scope as default.
            //ABP automatically saves all changes when a 'unit of work' scope ends (without any exception).
        }

        public void CreateCompany(Dtos.CreateCompanyInput input)
        {
            //We can use Logger, it's defined in ApplicationService class.
            //ERROR:  Logger.Info("Creating a task for input: " + input);

            //Creating a new Task entity with given input's properties
            var company = new Company { 
                Name = input.Name
            };

            //Saving entity with standard Insert method of repositories.
            _companyRepository.Insert(company);
        }
    }
}
