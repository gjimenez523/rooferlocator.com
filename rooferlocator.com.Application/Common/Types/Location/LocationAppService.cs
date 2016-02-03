using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using AutoMapper;
using rooferlocator.com.Common.Companies;
using Abp.Authorization;
using CreditsHero.Common.Dtos;
using System.IO;
using System.Net;


namespace rooferlocator.com.Common.Types
{
    public class LocationAppService : ApplicationService, ILocationAppService
    {
        //These members set in constructor using constructor injection.    
        private readonly ILocationTypeRepository _locationRepository;
        
        /// <summary>
        ///In constructor, we can get needed classes/interfaces.
        ///They are sent here by dependency injection system automatically.
        /// </summary>
        public LocationAppService(ILocationTypeRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public Dtos.GetLocationOutput GetLocations(Dtos.GetLocationInput input)
        {
            //Called specific GetAllWithPeople method of task repository.
            var types = _locationRepository.GetAll();

            //Used AutoMapper to automatically convert List<Task> to List<TaskDto>.
            //We must declare mappings to be able to use AutoMapper
            return new Dtos.GetLocationOutput
            {
                Locations = Mapper.Map<List<Dtos.LocationDto>>(types)
            };
        }

        /// <summary>
        /// Gets list of States
        /// </summary>
        /// <returns></returns>
        //[AbpAuthorize("RooferLocator.Permissions.Admin")]
        public Dtos.GetLocationOutput GetStates()
        {
            var states = _locationRepository.GetStates();
            return new Dtos.GetLocationOutput
            {
                Locations = Mapper.Map<List<Dtos.LocationDto>>(states)
            };
        }

        /// <summary>
        /// Gets list of Cities
        /// </summary>
        /// <returns></returns>
        public Dtos.GetLocationOutput GetCities(CreditsHero.Common.Dtos.GetCriteriaInput input, string state)
        {
            if (state != String.Empty)
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

                var cities = _locationRepository.GetCities(state);

                List<Dtos.LocationDto> locations = new List<Dtos.LocationDto>();

                locations = new Dtos.GetLocationOutput
                {
                    Locations = Mapper.Map<List<Dtos.LocationDto>>(cities)
                }.Locations;

                var query = (from cv in criteriaOutput.CriteriaValues
                             from l in locations
                             where l.City == cv.Name
                             select new Dtos.LocationDto
                             {
                                 City = l.City,
                                 CreationTime = l.CreationTime,
                                 CreatorUserId = l.CreatorUserId,
                                 State = l.State,
                                 LastModificationTime = l.LastModificationTime,
                                 LastModifierUserId = l.LastModifierUserId,
                                 Id = cv.Id,
                                 CreditCount = cv.CreditCount,
                                 CriteriaRefId = cv.CriteriaRefId,
                                 Name = cv.Name
                             }).ToList<Dtos.LocationDto>();

                return new Dtos.GetLocationOutput() { Locations = query };
            }
            else
            {
                return new Dtos.GetLocationOutput() { Locations = new List<Dtos.LocationDto>() };
            }
        }
        
        public void UpdateLocation(Dtos.UpdateLocationInput input)
        {
            //We can use Logger, it's defined in ApplicationService base class.
            //ERROR: Logger doesn't exist in ApplicationService base Logger.Info("Updating a task for input: " + input);

            //Retrieving a task entity with given id using standard Get method of repositories.
            var type = _locationRepository.Get(input.Id.Value);

            //Updating changed properties of the retrieved task entity.
            //if (input.FirstName != string.Empty) customer.FirstName = input.FirstName.Value;
            type.City = input.City;
            type.State = input.State;
            type.IsDeleted = input.IsDeleted;
                //customer.Id = _productRepository.Load(input.AssignedPersonId.Value);
                //customer.Purchases = _productRepository.Load(input.CustomerId.Value);
        
            //We even do not call Update method of the repository.
            //Because an application service method is a 'unit of work' scope as default.
            //ABP automatically saves all changes when a 'unit of work' scope ends (without any exception).
        }

        public void CreateLocation(Dtos.CreateLocationInput input)
        {
            //We can use Logger, it's defined in ApplicationService class.
            //ERROR:  Logger.Info("Creating a task for input: " + input);

            //Creating a new Task entity with given input's properties
            var type = new LocationType { 
                City = input.City,
                State = input.State
            };

            //Saving entity with standard Insert method of repositories.
            _locationRepository.Insert(type);
        }
    }
}
