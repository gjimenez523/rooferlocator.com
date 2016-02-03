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
    public class NewsAppService : ApplicationService, INewsAppService
    {
        //These members set in constructor using constructor injection.    
        private readonly INewsRepository _newsRepository;
        private readonly INewsCategoriesRepository _newsCategoriesRepository;
        
        /// <summary>
        ///In constructor, we can get needed classes/interfaces.
        ///They are sent here by dependency injection system automatically.
        /// </summary>
        public NewsAppService(INewsRepository newsRepository, INewsCategoriesRepository newsCategoriesRepository)
        {
            _newsRepository = newsRepository;
            _newsCategoriesRepository = newsCategoriesRepository;
        }
        public Dtos.GetNewsOutput GetNews(Dtos.GetNewsInput input)
        {
            //Called specific GetAllWithPeople method of task repository.
            var types = _newsRepository.GetAll();

            //Used AutoMapper to automatically convert List<Task> to List<TaskDto>.
            //We must declare mappings to be able to use AutoMapper
            return new Dtos.GetNewsOutput
            {
                News = Mapper.Map<List<Dtos.NewsDto>>(types)
            };
        }
        public Dtos.GetNewsCategoriesOutput GetNewsCategories(Dtos.GetNewsCategoriesInput input)
        {
            //Called specific GetAllWithPeople method of task repository.
            var types = _newsCategoriesRepository.GetAll();

            //Used AutoMapper to automatically convert List<Task> to List<TaskDto>.
            //We must declare mappings to be able to use AutoMapper
            return new Dtos.GetNewsCategoriesOutput
            {
                NewsCategories = Mapper.Map<List<Dtos.NewsCategoriesDto>>(types)
            };
        }
        public void UpdateNewsCategories(Dtos.UpdateNewsCategoriesInput input){
            var type = _newsCategoriesRepository.Get(input.Id.Value);

            type.Title = input.Title;
            type.IsDeleted = input.IsDeleted;
        }
        public void UpdateNews(Dtos.UpdateNewsInput input)
        {
            //We can use Logger, it's defined in ApplicationService base class.
            //ERROR: Logger doesn't exist in ApplicationService base Logger.Info("Updating a task for input: " + input);

            //Retrieving a task entity with given id using standard Get method of repositories.
            var type = _newsRepository.Get(input.Id.Value);

            //Updating changed properties of the retrieved task entity.
            //if (input.FirstName != string.Empty) customer.FirstName = input.FirstName.Value;
            type.Title = input.Title;
            type.Description = input.Description;
            type.NewsCategoriesRefId = input.NewsCategoriesRefId;
            type.IsDeleted = input.IsDeleted;
                //customer.Id = _productRepository.Load(input.AssignedPersonId.Value);
                //customer.Purchases = _productRepository.Load(input.CustomerId.Value);
        
            //We even do not call Update method of the repository.
            //Because an application service method is a 'unit of work' scope as default.
            //ABP automatically saves all changes when a 'unit of work' scope ends (without any exception).
        }
        public void CreateNews(Dtos.CreateNewsInput input)
        {
            //We can use Logger, it's defined in ApplicationService class.
            //ERROR:  Logger.Info("Creating a task for input: " + input);

            //Creating a new Task entity with given input's properties
            var type = new News { 
                Title = input.Title,
                Description = input.Description,
                NewsCategoriesRefId = input.NewsCategoriesRefId,
                IsDeleted = input.IsDeleted
            };

            //Saving entity with standard Insert method of repositories.
            _newsRepository.Insert(type);
        }
        public void CreateNewsCategories(Dtos.CreateNewsCategoriesInput input){
            var type = new NewsCategories
            {
                Title = input.Title,
                IsDeleted = input.IsDeleted
            };

            //Saving entity with standard Insert method of repositories.
            _newsCategoriesRepository.Insert(type);
        }
    }
}
