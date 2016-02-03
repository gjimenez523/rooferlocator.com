using System;
using Abp.Authorization.Users;
using Abp.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using rooferlocator.com.MultiTenancy;

namespace rooferlocator.com.Common.News
{
    [Table("RlNewsCategories")]
    public class NewsCategories : FullAuditedEntity
    {
        public string Title { get; set; }

        public NewsCategories() { }
    }
}