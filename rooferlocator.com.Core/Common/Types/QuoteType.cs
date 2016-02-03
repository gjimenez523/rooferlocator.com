using System;
using Abp.Authorization.Users;
using Abp.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using rooferlocator.com.MultiTenancy;

namespace rooferlocator.com.Common.Types
{
    [Table("RlQuoteType")]
    public class QuoteType : FullAuditedEntity
    {
        public string Quote { get; set; }
        public string DisplayName { get; set; }

        public QuoteType() { }
    }
}