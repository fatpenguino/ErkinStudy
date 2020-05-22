using System;
using System.Threading.Tasks;
using ErkinStudy.Infrastructure.DTOs;
using WooppayService;

namespace ErkinStudy.Infrastructure.ExternalServices
{
    public class WooppayPaymentService
    {
        public XmlControllerPortTypeClient CreateClient()
        {
            var client = new XmlControllerPortTypeClient();
            return client;
        }

        public async Task<string> Login()
        {
            try
            {
                var client = CreateClient();
                var request = new CoreLoginRequest {username = "test_merch", password = "A12345678a"};
                var response = await client.core_loginAsync(request);
                return response.response.session;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task Payment(PaymentRequestDto paymentRequestDto)
        {
            try
            {
                var client = CreateClient();
                var request = new CashCreateInvoiceExtended2Request();
                request.cardForbidden = 0;
                request.userEmail = paymentRequestDto.Email;
                request.userPhone = paymentRequestDto.PhoneNumber;
                request.backUrl = "https://localhost:44379/Home";
                request.requestUrl = $"https://localhost:44379/Payment/ConfirmOrder/{paymentRequestDto.Id}";
                request.addInfo = "";
                request.amount = (float) paymentRequestDto.Amount;
                request.deathDate = DateTime.Now.AddMinutes(15).ToString("yyyy-MM-dd hh:mm:ss");
                request.serviceType = 4;
                request.description = "";
                request.referenceId = paymentRequestDto.Id.ToString();
                var response = await client.cash_createInvoice2ExtendedAsync(request);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
