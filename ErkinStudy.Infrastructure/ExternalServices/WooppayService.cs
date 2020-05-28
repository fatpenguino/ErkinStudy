using System;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Payment;
using ErkinStudy.Infrastructure.DTOs;
using Microsoft.Extensions.Logging;
using WooppayService;

namespace ErkinStudy.Infrastructure.ExternalServices
{
    public class WooppayPaymentService
    {
        private readonly ILogger<WooppayPaymentService> _logger;

        public WooppayPaymentService(ILogger<WooppayPaymentService> logger)
        {
            _logger = logger;
        }

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
                var response = new OrderResponseDto();
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
                        serviceType = 5,
                        description = "",
                        referenceId = $"87078897741{orderRequest.OrderId}"
                    };
                    var wooppayResponse = await client.cash_createInvoice2ExtendedAsync(request);
                    response.ErrorCode = wooppayResponse.error_code;
                    if (response.ErrorCode == 0)
                    {
                        response.OperationId = wooppayResponse.response.operationId;
                        response.OperationUrl = wooppayResponse.response.operationUrl;
                    }
                    return new OrderResponseDto()
                        {ErrorCode = response.error_code, ErrorMessage = "Ошибка во время оплаты"};
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Ошибка при попытке оплаты, orderId - {orderRequest.OrderId}, {e}");
                return new OrderResponseDto() {ErrorCode = -1, ErrorMessage = "Ошибка при попытке оплаты"};
            }
        }

    }
}
