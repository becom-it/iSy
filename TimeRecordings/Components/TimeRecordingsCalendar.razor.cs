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
                    ChangeCalendar(value.year, value.month);
                    selectedMonth = value;
                }
            }
        }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public DateTime Current { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ChangeCalendar(DateTime.Now.Year, DateTime.Now.Month);
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

            Current = Start;

            _timeDetails = await ZeiterfassungsService.GetEmployeeTimeDetails(CompanyEnum.Austria, EmployeeId, Start, End);
        }

        public (TimeSpan netWorkTime, TimeSpan netWorkTimeDiff, string Info) GetDayInfo(DateTime theDay)
        {
            var info = _timeDetails.Where(x => x.PresenceDate.Date == theDay.Date).FirstOrDefault();
            if(info != null)
            {

            }
            return (TimeSpan.Zero, TimeSpan.Zero, "");
        }
    }
}