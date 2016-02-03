using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using AutoMapper;
using rooferlocator.com.Common.News;

namespace rooferlocator.com.Common.News
{
    public class TipsAppService : ApplicationService, ITipsAppService
    {
        //These members set in constructor using constructor injection.    
        private readonly ITipsRepository _tipsRepository;
        private readonly ITipsCategoriesRepository _tipsCategoriesRepository;
        
        /// <summary>
        ///In constructor, we can get needed classes/interfaces.
        ///They are sent here by dependency injection system automatically.
        /// </summary>
        public TipsAppService(ITipsRepository tipsRepository, ITipsCategoriesRepository tipsCategoriesRepository)
        {
            _tipsRepository = tipsRepository;
            _tipsCategoriesRepository = tipsCategoriesRepository;
        }
        public Dtos.GetTipsOutput GetTips(Dtos.GetTipsInput input)
        {
            //Called specific GetAllWithPeople method of task repository.
            var types = _tipsRepository.GetAll();

            //Used AutoMapper to automatically convert List<Task> to List<TaskDto>.
            //We must declare mappings to be able to use AutoMapper
            return new Dtos.GetTipsOutput
            {
                Tips = Mapper.Map<List<Dtos.TipsDto>>(types)
            };
        }
        public Dtos.GetTipsCategoriesOutput GetTipsCategories(Dtos.GetTipsCategoriesInput input)
        {
            //Called specific GetAllWithPeople method of task repository.
            var types = _tipsCategoriesRepository.GetAll();

            //Used AutoMapper to automatically convert List<Task> to List<TaskDto>.
            //We must declare mappings to be able to use AutoMapper
            return new Dtos.GetTipsCategoriesOutput
            {
                TipsCategories = Mapper.Map<List<Dtos.TipsCategoriesDto>>(types)
            };
        }
        public void UpdateTipsCategories(Dtos.UpdateTipsCategoriesInput input){
            var type = _tipsCategoriesRepository.Get(input.Id.Value);

            type.Title = input.Title;
            type.IsDeleted = input.IsDeleted;
        }
        public void UpdateTips(Dtos.UpdateTipsInput input)
        {
            //We can use Logger, it's defined in ApplicationService base class.
            //ERROR: Logger doesn't exist in ApplicationService base Logger.Info("Updating a task for input: " + input);

            //Retrieving a task entity with given id using standard Get method of repositories.
            var type = _tipsRepository.Get(input.Id.Value);

            //Updating changed properties of the retrieved task entity.
            //if (input.FirstName != string.Empty) customer.FirstName = input.FirstName.Value;
            type.Title = input.Title;
            type.Description = input.Description;
            type.TipsCategoriesRefId = input.TipsCategoriesRefId;
            type.IsDeleted = input.IsDeleted;
                //customer.Id = _productRepository.Load(input.AssignedPersonId.Value);
                //customer.Purchases = _productRepository.Load(input.CustomerId.Value);
        
            //We even do not call Update method of the repository.
            //Because an application service method is a 'unit of work' scope as default.
            //ABP automatically saves all changes when a 'unit of work' scope ends (without any exception).
        }
        public void CreateTips(Dtos.CreateTipsInput input)
        {
            //We can use Logger, it's defined in ApplicationService class.
            //ERROR:  Logger.Info("Creating a task for input: " + input);

            //Creating a new Task entity with given input's properties
            var type = new Tips { 
                Title = input.Title,
                Description = input.Description,
                TipsCategoriesRefId = input.TipsCategoriesRefId,
                IsDeleted = input.IsDeleted
            };

            //Saving entity with standard Insert method of repositories.
            _tipsRepository.Insert(type);
        }
        public void CreateTipsCategories(Dtos.CreateTipsCategoriesInput input){
            var type = new TipsCategories
            {
                Title = input.Title,
                IsDeleted = input.IsDeleted
            };

            //Saving entity with standard Insert method of repositories.
            _tipsCategoriesRepository.Insert(type);
        }
    }
}
