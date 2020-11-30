using Becom.EDI.PersonalDataExchange.Model;
using Becom.EDI.PersonalDataExchange.Model.Enums;
using Becom.EDI.PersonalDataExchange.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecordings.Components
{
    public partial class TimeRecordingsCheckins
    {
        [Inject]
        public IZeiterfassungsService ZeiterfassungsService { get; set; }

        [CascadingParameter]
        public CompanyEnum Company { get; set; }

        [CascadingParameter]
        public int EmployeeId { get; set; }

        private DateTime from = DateTime.Now;
        public DateTime From { get
            {
                return from;
            }
            set
            {
                if(from != value)
                {
                    from = value;
                    _ = update();
                }
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await update(true);
        }

        public List<EmployeeCheckIn> Checkins { get; set; }

        private async Task update(bool firstTime = false)
        {
            try
            {
                //Checkins = await ZeiterfassungsService.GetEmployeeCheckIns(Company, EmployeeId, From);

                if (!firstTime) StateHasChanged();
            }
            catch (Exception)
            {
                Checkins = null;
            }
        }
    }
}
