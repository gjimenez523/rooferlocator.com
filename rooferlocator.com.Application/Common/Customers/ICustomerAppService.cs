using Abp.Application.Services;
using rooferlocator.com.Common.Customers.Dtos;
using CreditsHero.Subscribers.Dtos;
using System.Collections.Generic;
using CreditsHero.Messaging.Dtos;
using CreditsHero.Subscribers.Dtos.PaymentGatewayDtos;
using CreditsHero.Common.Dtos;

namespace rooferlocator.com.Common.Customers
{
    public interface ICustomerAppService : IApplicationService
    {
        GetCustomersOutput GetCustomers(GetCustomerInput input);
        SubscribersInquiriesDto GetCustomerInquiries(GetSubscribersInput input);
        SubscriberQuotesDto GetCustomerQuotes(GetSubscribersInput input);
        NotificationResults SendEmail(NotificationInput input);
        void UpdateCustomer(UpdateCustomerInput input);
    }
}
