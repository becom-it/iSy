using Microsoft.AspNetCore.Components;
using System;
using iSy.Shared.Extensions;
using Becom.EDI.PersonalDataExchange.Services;
using System.Threading.Tasks;
using Becom.EDI.PersonalDataExchange.Model.Enums;
using Becom.EDI.PersonalDataExchange.Model;
using System.Collections.Generic;
using System.Linq;

namespace TimeRecordings.Components
{
    public partial class TimeRecordingsCalendar
    {
        private List<EmployeeTimeDetail> _timeDetails = new List<EmployeeTimeDetail>();

        [Inject]
        public IZeiterfassungsService ZeiterfassungsService { get; set; }

        [CascadingParameter]
        public int EmployeeId { get; set; }

        private (int month, int year) selectedMonth;
        public (int month, int year) SelectedMonth
        {
            get
            {
                return selectedMonth;
            }
            set
            {

                if (value != selectedMonth)
                {
                    //Task.Run(async () => await ChangeCalendar(value.year, value.month));
                    ChangeCalendar(value.year, value.month);
                    selectedMonth = value;
                }
            }
        }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public DateTime Current { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //await ChangeCalendar(DateTime.Now.Year, DateTime.Now.Month);
                SelectedMonth = (DateTime.Now.Month, DateTime.Now.Year);
            }
        }

        private async Task ChangeCalendar(int year, int month)
        {
            Start = new DateTime(year, month, 1);
            while (Start.DayOfWeek != DayOfWeek.Monday)
            {
                Start = Start.AddDays(-1);
            }

            End = new DateTime(year, month, 1).LastDayOfMonth();
            while (End.DayOfWeek != DayOfWeek.Sunday)
            {
                End = End.AddDays(1);
            }

            //Current = Start;

            _timeDetails = await ZeiterfassungsService.GetEmployeeTimeDetails(CompanyEnum.Austria, EmployeeId, Start, End);

            Current = Start;

            StateHasChanged();
        }

        public (TimeSpan netWorkTime, TimeSpan netWorkTimeDiff, string Info) GetDayInfo(DateTime theDay)
        {
            var info = _timeDetails.Where(x => x.PresenceDate.Date == theDay.Date).FirstOrDefault();
            if(info != null)
            {
                string adesc = ""; ;
                if (info.AbsentDescription1 != null)
                {
                    adesc = info.AbsentDescription1;
                    if (adesc.Length > 11) adesc = adesc.Substring(0, 11);
                }
                return (info.NetWorktime, info.NetWorktimeDifference, adesc);
            }
            return (TimeSpan.Zero, TimeSpan.Zero, "");
        }
    }
}
