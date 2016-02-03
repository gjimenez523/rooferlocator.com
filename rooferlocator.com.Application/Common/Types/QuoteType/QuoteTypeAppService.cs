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
    public class QuoteTypeAppService : ApplicationService, IQuoteTypeAppService
    {
        //These members set in constructor using constructor injection.    
        private readonly IQuoteTypeRepository _quoteTypeRepository;
        
        /// <summary>
        ///In constructor, we can get needed classes/interfaces.
        ///They are sent here by dependency injection system automatically.
        /// </summary>
        public QuoteTypeAppService(IQuoteTypeRepository quoteTypeRepository)
        {
            _quoteTypeRepository = quoteTypeRepository;
        }

        public Dtos.GetQuoteTypeOutput GetQuoteTypes(CreditsHero.Common.Dtos.GetCriteriaInput input)
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
            var types = _quoteTypeRepository.GetAll();

            //Used AutoMapper to automatically convert List<Task> to List<TaskDto>.
            //We must declare mappings to be able to use AutoMapper
            List<Dtos.QuoteTypeDto> quoteTypes = new List<Dtos.QuoteTypeDto>();

            quoteTypes = new Dtos.GetQuoteTypeOutput
                         {
                             QuoteTypes = Mapper.Map<List<Dtos.QuoteTypeDto>>(types)
                         }.QuoteTypes;

            var query = (from cv in criteriaOutput.CriteriaValues
                         from qt in quoteTypes
                         where qt.DisplayName == cv.Name
                         select new Dtos.QuoteTypeDto
                         {
                             Quote = qt.Quote,
                             CreationTime = qt.CreationTime,
                             CreatorUserId = qt.CreatorUserId,
                             DisplayName = qt.DisplayName,
                             LastModificationTime = qt.LastModificationTime,
                             LastModifierUserId = qt.LastModifierUserId,
                             Id = cv.Id,
                             CreditCount = cv.CreditCount,
                             CriteriaRefId = cv.CriteriaRefId,
                             Name = cv.Name
                         }).ToList<Dtos.QuoteTypeDto>();

            return new Dtos.GetQuoteTypeOutput() { QuoteTypes = query };
        }
        
        public void UpdateQuoteType(Dtos.UpdateQuoteTypeInput input)
        {
            //We can use Logger, it's defined in ApplicationService base class.
            //ERROR: Logger doesn't exist in ApplicationService base Logger.Info("Updating a task for input: " + input);

            //Retrieving a task entity with given id using standard Get method of repositories.
            var type = _quoteTypeRepository.Get(input.Id.Value);

            //Updating changed properties of the retrieved task entity.
            //if (input.FirstName != string.Empty) customer.FirstName = input.FirstName.Value;
            type.Quote = input.Quote;
            type.DisplayName = input.DisplayName;
            type.IsDeleted = input.IsDeleted;
                //customer.Id = _productRepository.Load(input.AssignedPersonId.Value);
                //customer.Purchases = _productRepository.Load(input.CustomerId.Value);
        
            //We even do not call Update method of the repository.
            //Because an application service method is a 'unit of work' scope as default.
            //ABP automatically saves all changes when a 'unit of work' scope ends (without any exception).
        }

        public void CreateQuoteType(Dtos.CreateQuoteTypeInput input)
        {
            //We can use Logger, it's defined in ApplicationService class.
            //ERROR:  Logger.Info("Creating a task for input: " + input);

            //Creating a new Task entity with given input's properties
            var type = new QuoteType { 
                Quote = input.Quote,
                DisplayName = input.DisplayName
            };

            //Saving entity with standard Insert method of repositories.
            _quoteTypeRepository.Insert(type);
        }
    }
}
