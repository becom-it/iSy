using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Becom.EDI.PersonalDataExchange.Tests.Helpers
{
    public static class HttpRequestMessageExtensions
    {
        public static bool CheckRequest(this HttpRequestMessage request, string target, string content)
        {
            return request.CheckMethod() && request.CheckUri(target) && request.CheckContent(content);
        }

        public static bool CheckMethod(this HttpRequestMessage request)
        {
            var res = request.Method == HttpMethod.Post;
            return res;
        }

        public static bool CheckUri(this HttpRequestMessage request, string target)
        {
            var res = request.RequestUri.AbsoluteUri == target;
            return res;
        }

        public static bool CheckContent(this HttpRequestMessage request, string content)
        {
            var test = request.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var res = test == content;
            return res;
        }
    }
}
