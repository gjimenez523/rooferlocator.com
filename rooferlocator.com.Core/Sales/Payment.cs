using System;
using Abp.Authorization.Users;
using Abp.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Users;
using rooferlocator.com.Common;

namespace rooferlocator.Sales
{
    [Table("RlPayments")]
    public class Payment : FullAuditedEntity
    {
        public string TransactionRefId { get; set; }
        public string TransactionDateEntered { get; set; }
        public string TransactionDateResponse { get; set; }
        public string TransactionAmount { get; set; }
        public string PONumber { get; set; }
        public string ReturnTransId { get; set; }
        public string ReturnRefNumber { get; set; }
        public string ReturnAuth { get; set; }
        public string ReturnAvsCode { get; set; }
        public string ReturnCVV2Response { get; set; }
        public string ReturnNotes { get; set; }
        public int MemberRefId { get; set; }

        [ForeignKey("MemberRefId")]
        public Member Member { get; set; }

        public Payment() { }
    }
}