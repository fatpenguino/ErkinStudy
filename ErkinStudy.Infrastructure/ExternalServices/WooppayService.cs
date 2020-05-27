using System;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Payment;
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

        public async Task<OrderResponseDto> Payment(OrderRequestDto orderRequest)
        {
            try
            {
                var client = CreateClient();
                var loginRequest = new CoreLoginRequest { username = "test_merch", password = "A12345678a" };
                var loginResponse = await client.core_loginAsync(loginRequest);
                if (loginResponse.error_code == 0)
                {
                    var request = new CashCreateInvoiceExtended2Request
                    {
                        cardForbidden = 0,
                        userEmail = orderRequest.Email,
                        userPhone = orderRequest.PhoneNumber,
                        backUrl = "https://localhost:44379/Home",
                        requestUrl = $"https://localhost:44379/Payment/ConfirmOrder/{orderRequest.OrderId}",
                        addInfo = "",
                        amount = (float)orderRequest.Amount,
                        deathDate = DateTime.Now.AddMinutes(15).ToString("yyyy-MM-dd hh:mm:ss"),
                        serviceType = 4,
                        description = "",
                        referenceId = orderRequest.OrderId.ToString()
                    };
                    var response = await client.cash_createInvoice2ExtendedAsync(request);
                    if (response.error_code == 0)
                        return new OrderResponseDto();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return null;
        }

    }
}
