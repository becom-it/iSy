using Becom.EDI.PersonalDataExchange.Model;
using Becom.EDI.PersonalDataExchange.Model.Enums;
using Becom.EDI.PersonalDataExchange.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TimeRecordings.Components
{
    public partial class TimeRecordingsInfo
    {
        [Inject]
        public ILogger<TimeRecordingsInfo> Logger { get; set; }

        [Inject]
        public IZeiterfassungsService zeiterfassungsService { get; set; }

        [CascadingParameter]
        public CompanyEnum Company { get; set; }

        [CascadingParameter]
        public int EmployeeId { get; set; }

        public EmployeeInfo Info { get; set; } = null;

        protected override async Task OnParametersSetAsync()
        {
            //Info = await zeiterfassungsService.GetEmployeeInfo(Company, EmployeeId);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender)
            {
                try
                {
                    Info = await zeiterfassungsService.GetEmployeeInfo(Company, EmployeeId);
                    StateHasChanged();
                }
                catch (System.Exception ex)
                {
                    Logger.LogError(ex.Message, ex);
                }
            }
        }
    }
}
