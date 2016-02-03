using System;
using Abp.Authorization.Users;
using Abp.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using rooferlocator.com.MultiTenancy;

namespace rooferlocator.com.Common
{
    [Table("RlTestimonial")]
    public class Testimonial : FullAuditedEntity
    {
        public string Details { get; set; }
        public string ImageUri { get; set; }

        public int MemberRefId { get; set; }

        [ForeignKey("MemberRefId")]
        public Member Member { get; set; }

        public Testimonial() { }
    }
}