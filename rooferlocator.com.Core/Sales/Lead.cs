using System;
using Abp.Authorization.Users;
using Abp.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Users;
using rooferlocator.com.Common.Types;

namespace rooferlocator.Sales
{
    [Table("RlLeads")]
    public class Lead : FullAuditedEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Budget { get; set; }
        public string Comment { get; set; }
        public DateTime TimeOfRepair { get; set; }
        public string State { get; set; }

        public int RoofTypeRefId { get; set; }
        public int ServiceTypeRefId { get; set; }
        public int LocationTypeRefId { get; set; }

        [ForeignKey("RoofTypeRefId")]
        public RoofType RoofType { get; set; }

        [ForeignKey("ServiceTypeRefId")]
        public ServiceType ServiceType { get; set; }
        
        [ForeignKey("LocationTypeRefId")]
        public LocationType LocationType { get; set; }

        public Lead() { }
    }
}