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
    [Table("RlRoofType")]
    public class RoofType : FullAuditedEntity
    {
        public string Code { get; set; }
        public string Description { get; set; }

        public RoofType() { }
    }
}