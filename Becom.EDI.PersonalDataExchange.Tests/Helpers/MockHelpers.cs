using Becom.EDI.PersonalDataExchange.Model.Config;
using Becom.EDI.PersonalDataExchange.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Becom.EDI.PersonalDataExchange.Tests.Helpers
{
    public class MockHelpers
    {
        public static (NullLogger<ZeiterfassungsService> logger, 
            Mock<IHttpClientFactory> mockFactory, 
            Mock<HttpMessageHandler> mockHttpMessageHandler, 
            PersonalDataExchangeConfig config) GetMocks(string result)
        {
            var logger = new NullLogger<ZeiterfassungsService>();
            var config = ConfigHelper.GetConfig();

            var mockFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => {
                    var msg = new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Content = new StringContent(result)
                    };
                    return msg;
                })
                .Verifiable();
            var client = new HttpClient(mockHttpMessageHandler.Object);
            client.BaseAddress = new Uri(config.Endpoint);

            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            
            

            return (logger, mockFactory, mockHttpMessageHandler, config);
        }
    }
}
