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
    [Table("RlMembers")]
    public class Member : FullAuditedEntity
    {
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Credential { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseDescription { get; set; }
        public long UserRefId { get; set; }
        public int CompanyRefId { get; set; }

        [ForeignKey("UserRefId")]
        public User User { get; set; }
        [ForeignKey("CompanyRefId")]
        public Company Company { get; set; }
        public List<Payment> Payments { get; set; }

        public Member() { }
    }

    [Table("RlMembersRoofType")]
    public class MembersRoofType : FullAuditedEntity
    {
        public int MemberRefId { get; set; }
        public int RoofTypeRefId { get; set; }

        [ForeignKey("MemberRefId")]
        public Member Member { get; set; }

        [ForeignKey("RoofTypeRefId")]
        public RoofType RoofType { get; set; }

        public MembersRoofType() { }
    }

    [Table("RlMembersServiceType")]
    public class MembersServiceType : FullAuditedEntity
    {
        public int MemberRefId { get; set; }
        public int ServiceTypeRefId { get; set; }

        [ForeignKey("MemberRefId")]
        public Member Member { get; set; }

        [ForeignKey("ServiceTypeRefId")]
        public RoofType ServiceType { get; set; }

        public MembersServiceType() { }
    }

    public class MemberVisits
    {
        public int VisitMonth { get; set; }
        public int VisitDay { get; set; }
        public int VisitYear { get; set; }
        public int ReturnVisits { get; set; }
        public int NewVisits { get; set; }
    }
}