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
    [Table("RlTips")]
    public class Tips : FullAuditedEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public int TipsCategoriesRefId { get; set; }

        [ForeignKey("TipsCategoriesRefId")]
        public TipsCategories TipsCategory { get; set; }

        public Tips() { }
    }
}