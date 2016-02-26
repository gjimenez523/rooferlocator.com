using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Users;
using CreditsHero.Subscribers.Dtos;
using rooferlocator.com.Common;
using System.Collections.Generic;

namespace rooferlocator.com.Web.Models
{
    public class ServiceResponse : IInputDto
    {
        public string FriendlyMessage { get; set; }
        public string StackTrace { get; set; }
        public List<string> Errors { get; set; }

        public ServiceResponse()
        {
        }
    }
}