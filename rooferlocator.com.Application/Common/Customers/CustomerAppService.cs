using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using AutoMapper;
using rooferlocator.com.Common;
using CreditsHero.Subscribers.Dtos;
using CreditsHero.Common.Dtos;
using CreditsHero.Messaging.Dtos;
using CreditsHero.Messaging.Requests.Dtos;
using System.IO;
using System.Net;
using CreditsHero.Subscribers.Dtos.PaymentGatewayDtos;
using CreditsHero.Customers.Dtos;

namespace rooferlocator.com.Common.Customers
{
    public class CustomerAppService : ApplicationService, ICustomerAppService
    {
        //These members set in constructor using constructor injection.    
        private readonly ICustomerRepository _customerRepository;
        private readonly ICreditsHeroConnect _creditsHeroConnect;
        private readonly Users.UserManager _userManager;

        /// <summary>
        ///In constructor, we can get needed classes/interfaces.
        ///They are sent here by dependency injection system automatically.
        /// </summary>
        public CustomerAppService(ICustomerRepository customerRepository,
            ICreditsHeroConnect creditsHeroConnect,
            Users.UserManager userManager)
        {
            _customerRepository = customerRepository;
            _creditsHeroConnect = creditsHeroConnect;
            _userManager = userManager;
        }

        public Dtos.GetCustomersOutput GetCustomers(Dtos.GetCustomerInput input)
        {
            if (input.CustomerId != null)
            {
                //TODO:  Get unique CreditsHero requests' emails for passed in email.
                var customers = _customerRepository.GetCustomersByUserId(input.CustomerId.Value);

                return new Dtos.GetCustomersOutput
                {
                    Customers = Mapper.Map<List<Dtos.CustomerDto>>(customers)
                };
            }
            else
            {
                if (input.CompanyId.HasValue)
                {
                    //TODO:  Get list of unique CreditsHero requests' emails.
                    CreditsHero.Customers.Dtos.GetCustomersInput inputCustomer = 
                        new CreditsHero.Customers.Dtos.GetCustomersInput() { CompanyId = input.CompanyId };
                    CreditsHero.Customers.Dtos.GetCustomersOutput results = new CreditsHero.Customers.Dtos.GetCustomersOutput();
                    var customersCreditsHero = (GetCustomersOutput)_creditsHeroConnect.CallCreditsHeroService<GetCustomersOutput>(
                        results, inputCustomer,
                        "api/services/app/Customer/GetCustomers");

                    var customerUsers = _userManager.Users.Where(u => results.Customers.Any(a => a.Email == u.EmailAddress));  //results.Customers.Where(c => _userManager.Users.Contains(c.Email));

                    Dtos.GetCustomersOutput customerList = new Dtos.GetCustomersOutput();
                    
                    foreach(var item in results.Customers)
                    {
                        Dtos.CustomerDto customer = new Dtos.CustomerDto();
                        customer.CellPhone = item.SmsNumber;
                        customer.Email = item.Email;
                        customer.FullName = item.FullName;
                        customerList.Customers.Add(customer);
                    }

                    return customerList;
                }
            }
            return null;
        }

        public CreditsHero.Subscribers.Dtos.SubscribersInquiriesDto GetCustomerInquiries(GetSubscribersInput input)
        {
            CreditsHero.Subscribers.Dtos.SubscribersInquiriesDto results = new CreditsHero.Subscribers.Dtos.SubscribersInquiriesDto();
            return (CreditsHero.Subscribers.Dtos.SubscribersInquiriesDto)_creditsHeroConnect.CallCreditsHeroService<SubscribersInquiriesDto>(results, input,
                "api/services/app/Subscriber/GetSubscribersInquiries");
        }

        public CreditsHero.Messaging.Dtos.SubscriberQuotesDto GetCustomerQuotes(GetSubscribersInput input)
        {
            GetQuotesInput inputQuote = new GetQuotesInput()
            {
                CompanyId = input.CompanyId.Value,
                SubscriberRefId = input.SubscribersId.Value,
                QuoteStatus = ""
            };
            CreditsHero.Messaging.Dtos.SubscriberQuotesDto results = new CreditsHero.Messaging.Dtos.SubscriberQuotesDto();
            return (CreditsHero.Messaging.Dtos.SubscriberQuotesDto)_creditsHeroConnect.CallCreditsHeroService<SubscriberQuotesDto>(results, inputQuote,
                "api/services/app/Quotes/GetSubscriberQuotesByStatus");
        }

        public NotificationResults SendEmail(NotificationInput input)
        {
            CreditsHero.Messaging.Dtos.NotificationResults results = new CreditsHero.Messaging.Dtos.NotificationResults();
            return (CreditsHero.Messaging.Dtos.NotificationResults)_creditsHeroConnect.CallCreditsHeroService<CreditsHero.Messaging.Dtos.NotificationResults>(results, input,
                "api/services/app/Notification/SendEmailNotification");
        }

        public void UpdateCustomer(Dtos.UpdateCustomerInput input)
        {
        }
    }
}
