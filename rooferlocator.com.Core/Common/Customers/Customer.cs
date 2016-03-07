using System;
using Abp.Authorization.Users;
using Abp.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Users;
using rooferlocator.Sales;
using rooferlocator.com.Common.Types;

namespace rooferlocator.com.Common
{
    public class Customer : Entity
    {
        public string FullName { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public long UserRefId { get; set; }
        
        [ForeignKey("UserRefId")]
        public User User { get; set; }
        
        public Customer() { }
    }
}