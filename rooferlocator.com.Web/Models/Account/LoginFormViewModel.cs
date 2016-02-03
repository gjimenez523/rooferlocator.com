using System.Collections.Generic;

namespace rooferlocator.com.Web.Models.Account
{
    public class LoginFormViewModel
    {
        public string ReturnUrl { get; set; }
        public bool IsMultiTenancyEnabled { get; set; }
        public List<CreditsHero.Common.Dtos.CriteriaValuesDto> RoofTypeValues { get; set; }
        public List<CreditsHero.Common.Dtos.CriteriaValuesDto> TypeOfService { get; set; }
        public List<CreditsHero.Common.Dtos.CriteriaValuesDto> TimeOfRepair { get; set; }
        public List<CreditsHero.Common.Dtos.CriteriaValuesDto> State { get; set; }
        public List<CreditsHero.Common.Dtos.CriteriaValuesDto> City { get; set; }
        public CreditsHero.Messaging.Requests.Dtos.GetInquiryResults InquiryResults { get; set; }
        public CreditsHero.Messaging.Dtos.RequestsDto RequestResults { get; set; }
    }
}