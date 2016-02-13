using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using AutoMapper;
using rooferlocator.com.Common.Companies;
using CreditsHero.Common.Companies.Dtos;
using System.IO;
using System.Net;
using CreditsHero.Company.Dtos;

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

        public CompanyDto GetCompany(CreditsHero.Common.Companies.Dtos.GetCompanyInput input)
        {
            var creditsHeroFormat = String.Format("{0}api/services/app/Company/GetCompany", System.Configuration.ConfigurationSettings.AppSettings["creditsHero:WebServiceApiPrefix"]);
            var timelineUrl = string.Format(creditsHeroFormat);
            CompanyDto companyResult;

            //Serialize object to JSON
            MemoryStream jsonStream = new MemoryStream();

            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(input);
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);

            HttpWebRequest creditsHeroRequest = (HttpWebRequest)WebRequest.Create(timelineUrl);
            creditsHeroRequest.ContentType = "application/json;charset=utf-8";
            creditsHeroRequest.ContentLength = byteArray.Length;
            creditsHeroRequest.Method = "POST";
            Stream newStream = creditsHeroRequest.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();
            WebResponse timeLineResponse = creditsHeroRequest.GetResponse();
            using (timeLineResponse)
            {
                using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                {
                    var results = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());

                    Newtonsoft.Json.Linq.JObject jObject2 = results.result;
                    var itemResult = Newtonsoft.Json.JsonConvert.DeserializeObject<CompanyDto>(jObject2.ToString());
                    companyResult = itemResult;
                }
            }
            return companyResult;
        }

        public CreditsHero.Company.Dtos.CompanyConfigsDto GetCompanyConfig(CreditsHero.Common.Companies.Dtos.GetCompanyInput input)
        {
            var creditsHeroFormat = String.Format("{0}api/services/app/Company/GetCompanyConfig", System.Configuration.ConfigurationSettings.AppSettings["creditsHero:WebServiceApiPrefix"]);
            var timelineUrl = string.Format(creditsHeroFormat);
            CompanyConfigsDto companyConfigList;

            //Serialize object to JSON
            MemoryStream jsonStream = new MemoryStream();

            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(input);
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);

            HttpWebRequest creditsHeroRequest = (HttpWebRequest)WebRequest.Create(timelineUrl);
            creditsHeroRequest.ContentType = "application/json;charset=utf-8";
            creditsHeroRequest.ContentLength = byteArray.Length;
            creditsHeroRequest.Method = "POST";
            Stream newStream = creditsHeroRequest.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();
            WebResponse timeLineResponse = creditsHeroRequest.GetResponse();
            using (timeLineResponse)
            {
                using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                {
                    var results = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());

                    Newtonsoft.Json.Linq.JObject jObject2 = results.result;
                    var itemResult = Newtonsoft.Json.JsonConvert.DeserializeObject<CompanyConfigsDto>(jObject2.ToString());
                    companyConfigList = itemResult;
                }
            }
            return companyConfigList;
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
