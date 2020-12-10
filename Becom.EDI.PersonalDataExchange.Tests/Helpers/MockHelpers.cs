using Becom.EDI.PersonalDataExchange.Model;
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
            PersonalDataExchangeConfig config,
            Mock<IIBMiSQLApi> sqlApi) GetMocks(string result)
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
            var client = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri(config.Endpoint)
            };

            mockFactory.Setup(_ => _.CreateClient(It.Is<string>((clientName) => clientName == "edi"))).Returns(client);
            
            

            return (logger, mockFactory, mockHttpMessageHandler, config, getSqlApiMock());
        }

        private static Mock<IIBMiSQLApi> getSqlApiMock()
        {
            var mockFactory = new Mock<IIBMiSQLApi>();

            mockFactory.Setup(x => x.ExecuteSQLStatement(It.Is<string>((sql) => sql == customizingRequest()))).Returns(Task.Run(() => customizingResult()));
            mockFactory.Setup(x => x.CallSqlService<ZeiterfassungsCustomizing>(It.Is<string>((sql) => sql == customizingRequest()))).Returns(Task.Run(() => getResult()));
            return mockFactory;
        }

        private static string customizingRequest() => @"{""Query"" : ""SELECT SUBSTRING(tgdata, 1, 1) as key, SUBSTRING(tgdata, 7, 26) as description FROM BEC001R426.DTG0LF dl where tgtart = 'ZEKZ'and TGTASL BETWEEN '1' AND '99'""}";

        private static string customizingResult() => @"{
  ""metrics"": {
    ""took"": ""0.223011139"",
    ""sqlTime"": ""0.218886625""
  },
  ""data"": [
    {
      ""00001"": ""F"",
      ""00002"": ""Freizeitoption            ""
    },
    {
      ""00001"": ""Ö"",
      ""00002"": ""ÖGB Weiterbildung         ""
    },
    {
      ""00001"": ""A"",
      ""00002"": ""Arztbesuch                ""
    },
    {
      ""00001"": ""D"",
      ""00002"": ""Dienstgang                ""
    },
    {
      ""00001"": ""V"",
      ""00002"": ""VAZ                       ""
    },
    {
      ""00001"": ""W"",
      ""00002"": ""Wehrdienst                ""
    },
    {
      ""00001"": ""W"",
      ""00002"": ""Wehrdienst                ""
    },
    {
      ""00001"": ""U"",
      ""00002"": ""Urlaub                    ""
    },
    {
      ""00001"": ""K"",
      ""00002"": ""Krank, Reha, Arbeitsunfall""
    },
    {
      ""00001"": ""K"",
      ""00002"": ""Krank, Reha, Arbeitsunfall""
    },
    {
      ""00001"": ""K"",
      ""00002"": ""Krank, Reha, Arbeitsunfall""
    },
    {
      ""00001"": ""F"",
      ""00002"": ""Freizeitoption            ""
    },
    {
      ""00001"": ""K"",
      ""00002"": ""Krank, Reha, Arbeitsunfall""
    },
    {
      ""00001"": ""K"",
      ""00002"": ""Krank, Reha, Arbeitsunfall""
    },
    {
      ""00001"": ""a"",
      ""00002"": ""Ansparausgleich           ""
    },
    {
      ""00001"": ""w"",
      ""00002"": ""Weiterbildung             ""
    },
    {
      ""00001"": ""k"",
      ""00002"": ""Karenz                    ""
    },
    {
      ""00001"": ""S"",
      ""00002"": ""Sonstige Abwesenheit      ""
    },
    {
      ""00001"": ""S"",
      ""00002"": ""Sonstige Abwesenheit      ""
    },
    {
      ""00001"": ""S"",
      ""00002"": ""Sonstige Abwesenheit      ""
    },
    {
      ""00001"": ""S"",
      ""00002"": ""Sonstige Abwesenheit      ""
    },
    {
      ""00001"": ""S"",
      ""00002"": ""Sonstige Abwesenheit      ""
    },
    {
      ""00001"": ""F"",
      ""00002"": ""Freizeitoption            ""
    },
    {
      ""00001"": ""S"",
      ""00002"": ""Sonstige Abwesenheit      ""
    },
    {
      ""00001"": ""S"",
      ""00002"": ""Sonstige Abwesenheit      ""
    },
    {
      ""00001"": ""S"",
      ""00002"": ""Sonstige Abwesenheit      ""
    },
    {
      ""00001"": ""S"",
      ""00002"": ""Sonstige Abwesenheit      ""
    },
    {
      ""00001"": ""S"",
      ""00002"": ""Sonstige Abwesenheit      ""
    },
    {
      ""00001"": ""S"",
      ""00002"": ""Sonstige Abwesenheit      ""
    },
    {
      ""00001"": ""P"",
      ""00002"": ""Pause                     ""
    },
    {
      ""00001"": ""P"",
      ""00002"": ""Pause                     ""
    },
    {
      ""00001"": ""P"",
      ""00002"": ""Pause                     ""
    },
    {
      ""00001"": ""P"",
      ""00002"": ""Pause                     ""
    },
    {
      ""00001"": ""U"",
      ""00002"": ""Urlaub                    ""
    },
    {
      ""00001"": ""P"",
      ""00002"": ""Pause                     ""
    },
    {
      ""00001"": ""P"",
      ""00002"": ""Pause                     ""
    },
    {
      ""00001"": ""U"",
      ""00002"": ""Urlaub                    ""
    },
    {
      ""00001"": ""U"",
      ""00002"": ""Urlaub                    ""
    },
    {
      ""00001"": ""U"",
      ""00002"": ""Urlaub                    ""
    },
    {
      ""00001"": ""U"",
      ""00002"": ""Urlaub                    ""
    },
    {
      ""00001"": ""Ö"",
      ""00002"": ""ÖGB Weiterbildung         ""
    }
  ]
}";

        private static List<ZeiterfassungsCustomizing> getResult()
        {
            var ret = new List<ZeiterfassungsCustomizing>
            {
                new ZeiterfassungsCustomizing { AbscenceKey = "F", Description = "Freizeitoption" },
                new ZeiterfassungsCustomizing { AbscenceKey = "F", Description = "Freizeitoption" },
                new ZeiterfassungsCustomizing { AbscenceKey = "Ö", Description = "ÖGB Weiterbildung" },
                new ZeiterfassungsCustomizing { AbscenceKey = "A", Description = "Arztbesuch" },
                new ZeiterfassungsCustomizing { AbscenceKey = "D", Description = "Dienstgang" },
                new ZeiterfassungsCustomizing { AbscenceKey = "V", Description = "VAZ" },
                new ZeiterfassungsCustomizing { AbscenceKey = "W", Description = "Wehrdienst" },
                new ZeiterfassungsCustomizing { AbscenceKey = "W", Description = "Wehrdienst" },
                new ZeiterfassungsCustomizing { AbscenceKey = "U", Description = "Urlaub" },
                new ZeiterfassungsCustomizing { AbscenceKey = "K", Description = "Krank, Reha, Arbeitsunfall" },
                new ZeiterfassungsCustomizing { AbscenceKey = "K", Description = "Krank, Reha, Arbeitsunfall" },
                new ZeiterfassungsCustomizing { AbscenceKey = "K", Description = "Krank, Reha, Arbeitsunfall" },
                new ZeiterfassungsCustomizing { AbscenceKey = "F", Description = "Freizeitoption" },
                new ZeiterfassungsCustomizing { AbscenceKey = "K", Description = "Krank, Reha, Arbeitsunfall" },
                new ZeiterfassungsCustomizing { AbscenceKey = "K", Description = "Krank, Reha, Arbeitsunfall" },
                new ZeiterfassungsCustomizing { AbscenceKey = "a", Description = "Ansparausgleich" },
                new ZeiterfassungsCustomizing { AbscenceKey = "w", Description = "Weiterbildung" },
                new ZeiterfassungsCustomizing { AbscenceKey = "k", Description = "Karenz" },
                new ZeiterfassungsCustomizing { AbscenceKey = "S", Description = "Sonstige Abwesenheit" },
                new ZeiterfassungsCustomizing { AbscenceKey = "S", Description = "Sonstige Abwesenheit" },
                new ZeiterfassungsCustomizing { AbscenceKey = "S", Description = "Sonstige Abwesenheit" },
                new ZeiterfassungsCustomizing { AbscenceKey = "S", Description = "Sonstige Abwesenheit" },
                new ZeiterfassungsCustomizing { AbscenceKey = "S", Description = "Sonstige Abwesenheit" },
                new ZeiterfassungsCustomizing { AbscenceKey = "F", Description = "Freizeitoption" },
                new ZeiterfassungsCustomizing { AbscenceKey = "S", Description = "Sonstige Abwesenheit" },
                new ZeiterfassungsCustomizing { AbscenceKey = "S", Description = "Sonstige Abwesenheit" },
                new ZeiterfassungsCustomizing { AbscenceKey = "S", Description = "Sonstige Abwesenheit" },
                new ZeiterfassungsCustomizing { AbscenceKey = "S", Description = "Sonstige Abwesenheit" },
                new ZeiterfassungsCustomizing { AbscenceKey = "S", Description = "Sonstige Abwesenheit" },
                new ZeiterfassungsCustomizing { AbscenceKey = "S", Description = "Sonstige Abwesenheit" },
                new ZeiterfassungsCustomizing { AbscenceKey = "P", Description = "Pause" },
                new ZeiterfassungsCustomizing { AbscenceKey = "P", Description = "Pause" },
                new ZeiterfassungsCustomizing { AbscenceKey = "P", Description = "Pause" },
                new ZeiterfassungsCustomizing { AbscenceKey = "P", Description = "Pause" },
                new ZeiterfassungsCustomizing { AbscenceKey = "U", Description = "Urlaub" },
                new ZeiterfassungsCustomizing { AbscenceKey = "P", Description = "Pause" },
                new ZeiterfassungsCustomizing { AbscenceKey = "P", Description = "Pause" },
                new ZeiterfassungsCustomizing { AbscenceKey = "U", Description = "Urlaub" },
                new ZeiterfassungsCustomizing { AbscenceKey = "U", Description = "Urlaub" },
                new ZeiterfassungsCustomizing { AbscenceKey = "U", Description = "Urlaub" },
                new ZeiterfassungsCustomizing { AbscenceKey = "U", Description = "Urlaub" },
                new ZeiterfassungsCustomizing { AbscenceKey = "Ö", Description = "ÖGB Weiterbildung" }
            };
            return ret;
        }

    }
}
