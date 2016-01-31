using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace rooferlocator.com.MultiTenancy.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantListDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}