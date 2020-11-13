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
                EmployeeTimeDetailsRequest = RequestContents.GetEmployeeTimeDetailsRequestConfig()
            };
        }
    }
}
