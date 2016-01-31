using System;
using Abp.Authorization.Users;
using Abp.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using rooferlocator.com.MultiTenancy;

namespace rooferlocator.Common.Types
{
    [Table("RlLocationType")]
    public class LocationType : FullAuditedEntity
    {
        public string State { get; set; }
        public string City { get; set; }

        public LocationType() { }
    }
}