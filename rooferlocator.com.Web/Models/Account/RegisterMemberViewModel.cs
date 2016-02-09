using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Users;
using CreditsHero.Subscribers.Dtos;
using rooferlocator.com.Common;

namespace rooferlocator.com.Web.Models.Account
{
    public class RegisterMemberViewModel : IInputDto
    {
        /// <summary>
        /// Not required for single-tenant applications.
        /// </summary>
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string Fax { get; set; }
        [EmailAddress]
        [StringLength(User.MaxEmailAddressLength)]
        public string Email { get; set; }
        public string Credential { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseDescription { get; set; }
        public long UserRefId { get; set; }
        public Company Company { get; set; }

        public RegisterMemberViewModel()
        {
            Company = new Company();
        }
    }
}