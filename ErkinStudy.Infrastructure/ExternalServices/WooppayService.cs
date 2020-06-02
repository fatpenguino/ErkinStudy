using System;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Payment;
using ErkinStudy.Infrastructure.DTOs;
using ErkinStudy.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using WooppayService;

namespace ErkinStudy.Infrastructure.ExternalServices
{
    public class WooppayPaymentService
    {
        private readonly ILogger<WooppayPaymentService> _logger;
        private readonly OrderService _orderService;
        public WooppayPaymentService(ILogger<WooppayPaymentService> logger, OrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
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
            var response = new OrderResponseDto();
            try
            {
                var client = CreateClient();
                var loginRequest = new CoreLoginRequest { username = "test_merch", password = "A12345678a" };
                var loginResponse = await client.core_loginAsync(loginRequest);
                if (loginResponse.error_code == 0)
                {
                    var hash = _orderService.GetHash(orderRequest.OrderId);
                    var request = new CashCreateInvoiceExtended2Request
                    {
                        cardForbidden = 0,
                        userEmail = orderRequest.Email,
                        userPhone = orderRequest.PhoneNumber,
                        backUrl = "https://bolme.kz/SuccessPage",
                        requestUrl = $"https://bolme.kz/ConfirmPayment?orderId={orderRequest.OrderId}&hash={hash}",
                        addInfo = "",
                        amount = (float)orderRequest.Amount,
                        deathDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd hh:mm:ss"),
                        serviceType = 5,
                        description = "",
                        referenceId = $"bolme{orderRequest.OrderId}",
                        orderNumber = (int)orderRequest.OrderId
                    };
                    var wooppayResponse = await client.cash_createInvoice2ExtendedAsync(request);
                    response.ErrorCode = wooppayResponse.error_code;
                    if (response.ErrorCode == 0)
                    {
                        response.OperationId = wooppayResponse.response.operationId;
                        response.OperationUrl = wooppayResponse.response.operationUrl;
                    }
                    else
                    {
                        response.ErrorCode = wooppayResponse.error_code;
                        response.ErrorMessage = "";
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Ошибка при попытке оплаты, orderId - {orderRequest.OrderId}, {e}");
                response.ErrorCode = -1;
                response.ErrorMessage = "Ошибка при попытке оплаты";
            }
            return response;
        }

    }
}
