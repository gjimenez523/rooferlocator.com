using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using AutoMapper;
using rooferlocator.com.Common.Companies;
using CreditsHero.Common.Dtos;
using System.IO;
using System.Net;


namespace rooferlocator.com.Common.Types
{
    public class ServiceTypeAppService : ApplicationService, IServiceTypeAppService
    {
        //These members set in constructor using constructor injection.    
        private readonly IServiceTypeRepository _serviceTypeRepository;
        
        /// <summary>
        ///In constructor, we can get needed classes/interfaces.
        ///They are sent here by dependency injection system automatically.
        /// </summary>
        public ServiceTypeAppService(IServiceTypeRepository serviceTypeRepository)
        {
            _serviceTypeRepository = serviceTypeRepository;
        }

        public Dtos.GetServiceTypeOutput GetServiceTypes(CreditsHero.Common.Dtos.GetCriteriaInput input)
        {
            #region CreditsHero CriteriaValues
            var creditsHeroFormat = String.Format("{0}api/services/app/Criteria/GetCriteriaValues", System.Configuration.ConfigurationSettings.AppSettings["creditsHero:WebServiceApiPrefix"]);
            //var creditsHeroFormat = "http://creditshero.azurewebsites.net/api/services/cd/Criteria/GetCriteriaValues";
            //var creditsHeroFormat = "http://localhost:6234/api/services/cd/Criteria/GetCriteriaValues";
            var timelineUrl = string.Format(creditsHeroFormat);
            CreditsHero.Common.Dtos.GetCriteriaValuesOutput criteriaOutput = new CreditsHero.Common.Dtos.GetCriteriaValuesOutput();

            //Serialize object to JSON
            MemoryStream jsonStream = new MemoryStream();
            CreditsHero.Common.Dtos.GetCriteriaInput criteriaInput = new CreditsHero.Common.Dtos.GetCriteriaInput()
            {
                CompanyId = input.CompanyId,
                CriteriaId = input.CriteriaId
            };
            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(criteriaInput);
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
                    criteriaOutput.CriteriaValues = new List<CreditsHero.Common.Dtos.CriteriaValuesDto>();
                    foreach (var item in results.result.criteriaValues)
                    {
                        criteriaOutput.CriteriaValues.Add(
                            Newtonsoft.Json.JsonConvert.DeserializeObject<CreditsHero.Common.Dtos.CriteriaValuesDto>(item.ToString()));
                    }
                }
            }
            #endregion

            //Called specific GetAllWithPeople method of task repository.
            var types = _serviceTypeRepository.GetAll();

            //Used AutoMapper to automatically convert List<Task> to List<TaskDto>.
            //We must declare mappings to be able to use AutoMapper

            List<Dtos.ServiceTypeDto> serviceTypes = new List<Dtos.ServiceTypeDto>();

            serviceTypes = new Dtos.GetServiceTypeOutput
                         {
                             ServiceTypes = Mapper.Map<List<Dtos.ServiceTypeDto>>(types)
                         }.ServiceTypes;

            var query = (from cv in criteriaOutput.CriteriaValues
                         from st in serviceTypes
                         where st.Description == cv.Name
                         select new Dtos.ServiceTypeDto
                         {
                             Code = st.Code,
                             CreationTime = st.CreationTime,
                             CreatorUserId = st.CreatorUserId,
                             Description = st.Description,
                             LastModificationTime = st.LastModificationTime,
                             LastModifierUserId = st.LastModifierUserId,
                             Id = cv.Id,
                             CreditCount = cv.CreditCount,
                             CriteriaRefId = cv.CriteriaRefId,
                             Name = cv.Name
                         }).ToList<Dtos.ServiceTypeDto>();

            return new Dtos.GetServiceTypeOutput() { ServiceTypes = query };
            
        }
        
        public void UpdateServiceType(Dtos.UpdateServiceTypeInput input)
        {
            //We can use Logger, it's defined in ApplicationService base class.
            //ERROR: Logger doesn't exist in ApplicationService base Logger.Info("Updating a task for input: " + input);

            //Retrieving a task entity with given id using standard Get method of repositories.
            var type = _serviceTypeRepository.Get(input.Id.Value);

            //Updating changed properties of the retrieved task entity.
            //if (input.FirstName != string.Empty) customer.FirstName = input.FirstName.Value;
            type.Code = input.Code;
            type.Description = input.Description;
            type.IsDeleted = input.IsDeleted;
                //customer.Id = _productRepository.Load(input.AssignedPersonId.Value);
                //customer.Purchases = _productRepository.Load(input.CustomerId.Value);
        
            //We even do not call Update method of the repository.
            //Because an application service method is a 'unit of work' scope as default.
            //ABP automatically saves all changes when a 'unit of work' scope ends (without any exception).
        }

        public void CreateServiceType(Dtos.CreateServiceTypeInput input)
        {
            //We can use Logger, it's defined in ApplicationService class.
            //ERROR:  Logger.Info("Creating a task for input: " + input);

            //Creating a new Task entity with given input's properties
            var type = new ServiceType { 
                Code = input.Code,
                Description = input.Description
            };

            //Saving entity with standard Insert method of repositories.
            _serviceTypeRepository.Insert(type);

            #region CreditsHero CriteriaValues
            var creditsHeroFormat = String.Format("{0}api/services/app/Criteria/AddCriteriaValue", System.Configuration.ConfigurationSettings.AppSettings["creditsHero:WebServiceApiPrefix"]);
            //var creditsHeroFormat = "http://creditshero.azurewebsites.net/api/services/cd/Criteria/AddCriteriaValue";
            //var creditsHeroFormat = "http://localhost:6234/api/services/cd/Criteria/AddCriteriaValue";
            var timelineUrl = string.Format(creditsHeroFormat);
            CreditsHero.Common.Dtos.CriteriaValuesDto criteriaOutput = new CreditsHero.Common.Dtos.CriteriaValuesDto();

            //Serialize object to JSON
            MemoryStream jsonStream = new MemoryStream();
            CreditsHero.Common.Dtos.CreateCriteriaValuesInput criteriaInput = new CreditsHero.Common.Dtos.CreateCriteriaValuesInput()
            {
                CreditCount = input.CreditCount,
                CriteriaRefId = input.CriteriaRefId,
                CriteriaValuesId = 0,
                Name = input.Description
            };
            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(criteriaInput);
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
                    criteriaOutput = new CreditsHero.Common.Dtos.CriteriaValuesDto();
                }
            }
            #endregion
        }
    }
}
