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
    public class RoofTypeAppService : ApplicationService, IRoofTypeAppService
    {
        //These members set in constructor using constructor injection.    
        private readonly IRoofTypeRepository _roofTypeRepository;
        
        /// <summary>
        ///In constructor, we can get needed classes/interfaces.
        ///They are sent here by dependency injection system automatically.
        /// </summary>
        public RoofTypeAppService(IRoofTypeRepository roofTypeRepository)
        {
            _roofTypeRepository = roofTypeRepository;
        }

        public Dtos.GetRoofTypeOutput GetRoofTypes(CreditsHero.Common.Dtos.GetCriteriaInput input)
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
            var types = _roofTypeRepository.GetAll();

            List<Dtos.RoofTypeDto> roofTypes = new List<Dtos.RoofTypeDto>();

            roofTypes = new Dtos.GetRoofTypeOutput
                         {
                             RoofTypes = Mapper.Map<List<Dtos.RoofTypeDto>>(types)
                         }.RoofTypes;

            //var mergeResults = criteriaOutput.CriteriaValues.Select(s => new Dtos.RoofTypeDto { Name = s.Name, Id = s.Id, CreditCount = s.CreditCount, CriteriaRefId = s.CriteriaRefId }).ToList();
            //mergeResults.AddRange(roofTypes.Select(s => new Dtos.RoofTypeDto{Name = s.Name, CreditCount = s.CreditCount, CriteriaRefId = s.CriteriaRefId, Id = s.Id, Code = s.Code, CreationTime = s.CreationTime, CreatorUserId = s.CreatorUserId, Description = s.Description, LastModificationTime = s.LastModificationTime, LastModifierUserId = s.LastModifierUserId}).ToList());
            //List<CreditsHero.Common.Dtos.CriteriaValuesDto> roofTypeList = roofTypes.Concat(criteriaOutput.CriteriaValues).ToList();

            var query = (from cv in criteriaOutput.CriteriaValues
                        from rt in roofTypes
                        where rt.Description == cv.Name
                        select new Dtos.RoofTypeDto
                        {
                            Code = rt.Code,
                            CreationTime = rt.CreationTime,
                            CreatorUserId = rt.CreatorUserId,
                            Description = rt.Description,
                            LastModificationTime = rt.LastModificationTime,
                            LastModifierUserId = rt.LastModifierUserId,
                            Id = cv.Id,
                            CreditCount = cv.CreditCount,
                            CriteriaRefId = cv.CriteriaRefId,
                            Name = cv.Name
                        }).ToList<Dtos.RoofTypeDto>();

            return new Dtos.GetRoofTypeOutput() { RoofTypes = query };
            //roofTypeResults = roofTypeResults.Union(criteriaOutput.CriteriaValues);
        }
        
        public void UpdateRoofType(Dtos.UpdateRoofTypeInput input)
        {
            //We can use Logger, it's defined in ApplicationService base class.
            //ERROR: Logger doesn't exist in ApplicationService base Logger.Info("Updating a task for input: " + input);

            //Retrieving a task entity with given id using standard Get method of repositories.
            var type = _roofTypeRepository.Get(input.Id.Value);

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

        public void CreateRoofType(Dtos.CreateRoofTypeInput input)
        {
            //We can use Logger, it's defined in ApplicationService class.
            //ERROR:  Logger.Info("Creating a task for input: " + input);

            //Creating a new Task entity with given input's properties
            var type = new RoofType { 
                Code = input.Code,
                Description = input.Description
            };

            //Saving entity with standard Insert method of repositories.
            _roofTypeRepository.Insert(type);

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
