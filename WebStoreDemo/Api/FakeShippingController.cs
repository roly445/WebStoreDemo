using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebStoreDemo.Infrastucture.Extensions;

namespace WebStoreDemo.Api
{
    public class FakeShippingController : Controller
    {
        [HttpPost]
        [Route("api/fake-shipping-provider")]
        public IList<ShippingMethod> GetShippingMethods([FromBody]ShippingMethodRequest shippingMethodRequest)
        {
            var initialCost = shippingMethodRequest.PostalCode.GenerateDecimal();
            var shippingMethods = new List<ShippingMethod>{
                new ShippingMethod{
                    Id = "economy",
                    Label = "Economy Shipping (5-7 Days)",
                    Value = initialCost / 2
                },
                new ShippingMethod{
                    Id = "express",
                    Label = "Express Shipping (2-3 Days)",
                    Value = initialCost
                },
                new ShippingMethod{
                    Id = "next-day",
                    Label = "Next Day Delivery",
                    Value = initialCost * 2
                },
            };

            return shippingMethods;
        }
    }

    public class ShippingMethodRequest
    {
        public string PostalCode { get;set;}
    }

    public class ShippingMethod
    {
        public string Id {get;set;}
        public string Label { get;set;}
        public decimal Value {get;set;}
    }

    
}