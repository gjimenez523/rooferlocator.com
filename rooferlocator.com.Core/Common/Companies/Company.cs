using System;
using Abp.Authorization.Users;
using Abp.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Users;

namespace rooferlocator.Common
{
    [Table("RlCompany")]
    public class Company : FullAuditedEntity
    {
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ServiceArea { get; set; }
        public string YearEstablished { get; set; }

        public Company() { }
    }
}