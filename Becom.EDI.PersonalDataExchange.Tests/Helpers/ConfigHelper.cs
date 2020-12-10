using Becom.EDI.PersonalDataExchange.Model.Config;

namespace Becom.EDI.PersonalDataExchange.Tests.Helpers
{
    public class ConfigHelper
    {
        public static PersonalDataExchangeConfig GetConfig()
        {
            return new PersonalDataExchangeConfig
            {
                Endpoint = "http://hitsrvtedi2:20304/PersonalDataExchange",
                EmployeeInfoRequest = RequestContents.GetEmployeeInfoRequestConfig(),
                EmployeeCheckInsRequest = RequestContents.GetEmployeeCheckInsRequestConfig(),
                EmployeeListRequest = RequestContents.GetEmployeeListRequestConfig(),
                EmployeePresenceStatusRequest = RequestContents.GetEmployeePresenceStatusRequestConfig(),
                EmployeeTimeDetailsRequest = RequestContents.GetEmployeeTimeDetailsRequestConfig(),
                ZeiterfassungsCustomizingQuery = @"{""Query"" : ""SELECT SUBSTRING(tgdata, 1, 1) as key, SUBSTRING(tgdata, 7, 26) as description FROM BEC001R426.DTG0LF dl where tgtart = 'ZEKZ'and TGTASL BETWEEN '1' AND '99'""}"
        };
        }
    }
}
