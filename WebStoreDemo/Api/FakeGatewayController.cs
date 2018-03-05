using System;
using Microsoft.AspNetCore.Mvc;
using WebStoreDemo.Infrastucture.Repositories;

namespace WebStoreDemo.Api
{
    [Produces("application/json")]
    public class FakeGatewayController : Controller
    {
        private readonly ICacheDbRepository _cacheDbRepository;

        public FakeGatewayController(ICacheDbRepository cacheDbRepository)
        {
            this._cacheDbRepository = cacheDbRepository ?? throw new ArgumentNullException(nameof(cacheDbRepository));
        }

        [Route("api/gateway/make-card-payment")]
        [HttpPost]
        public IActionResult MakeCardPayment([FromBody]PaymentRequest paymentRequest)
        {
            var payment = new Payment
            {
                WhenRecieved = DateTime.UtcNow,
                PaymentRequest = paymentRequest
            };

            this._cacheDbRepository.AddToCacheDb(payment, "Payments");
            return Ok();
        }
    }

    public class Payment
    {
        public int Id { get; set; }
        public DateTime WhenRecieved { get; set; }
        public PaymentRequest PaymentRequest { get; set; }
    }

public class PaymentRequest
{
    public string requestId { get; set; }
        public string methodName { get; set; }
        public Details details { get; set; }
        public object shippingAddress { get; set; }
        public object shippingOption { get; set; }
        public object payerName { get; set; }
        public object payerEmail { get; set; }
        public object payerPhone { get; set; }
}

public class Details
    {
        public Billingaddress billingAddress { get; set; }
        public string cardNumber { get; set; }
        public string cardSecurityCode { get; set; }
        public string cardholderName { get; set; }
        public string expiryMonth { get; set; }
        public string expiryYear { get; set; }
    }

    public class Billingaddress
    {
        public string[] addressLine { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string dependentLocality { get; set; }
        public string languageCode { get; set; }
        public string organization { get; set; }
        public string phone { get; set; }
        public string postalCode { get; set; }
        public string recipient { get; set; }
        public string region { get; set; }
        public string sortingCode { get; set; }
    }

}