using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services;
using AutoMapper;
using CreditsHero.Subscribers.Dtos;
using CreditsHero.Messaging.Dtos;
using System.IO;
using System.Net;
using CreditsHero.Subscribers.Dtos.PaymentGatewayDtos;
using System.Security.Claims;
using System.Threading.Tasks;

namespace rooferlocator.com.Common
{
    public class CreditsHeroConnect : ICreditsHeroConnect
    {
        /// <summary>
        ///In constructor, we can get needed classes/interfaces.
        ///They are sent here by dependency injection system automatically.
        /// </summary>
        public CreditsHeroConnect()
        {
        }

        public object CallCreditsHeroServiceSecured<T>(object results, object input, string servicePostFix)
        {
            return CallCreditsHeroService<T>(
                results, input, servicePostFix,
                System.Configuration.ConfigurationSettings.AppSettings["creditsHero:WebServiceApiPrefixSsl"]);
        }

        public object CallCreditsHeroService<T>(object results, object input, string servicePostFix)
        {
            return CallCreditsHeroService<T>(
                results, input, servicePostFix,
                System.Configuration.ConfigurationSettings.AppSettings["creditsHero:WebServiceApiPrefix"]);
        }

        public object CallCreditsHeroService<T>(object results, object input, string servicePostFix, string servicePreFix)
        {
            try
            {
                var creditsHeroFormat = String.Format("{0}{1}",
                    servicePreFix,
                    servicePostFix);
                var timelineUrl = string.Format(creditsHeroFormat);

                //Serialize object to JSON
                MemoryStream jsonStream = new MemoryStream();

                string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(input);
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);

                HttpWebRequest creditsHeroRequest = (HttpWebRequest)WebRequest.Create(timelineUrl);
                creditsHeroRequest.ContentType = "application/json;charset=utf-8";
                creditsHeroRequest.ContentLength = byteArray.Length;
                creditsHeroRequest.Method = "POST";
                Stream newStream = creditsHeroRequest.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);
                newStream.Close();
                WebResponse timeLineResponse = creditsHeroRequest.GetResponse();
                using (timeLineResponse)
                {
                    using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                    {
                        var serviceResults = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());

                        Newtonsoft.Json.Linq.JObject jObject2 = serviceResults.result;
                        if (jObject2 != null)
                        {
                            var itemResult = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jObject2.ToString());
                            results = itemResult;
                            return results;
                        }
                    }
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
            return null;
        }
    }
}
