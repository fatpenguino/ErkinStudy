using System;
using System.Threading.Tasks;
using WooppayService;

namespace ErkinStudy.Infrastructure.ExternalServices
{
    public class WooppayService
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

    }
}
