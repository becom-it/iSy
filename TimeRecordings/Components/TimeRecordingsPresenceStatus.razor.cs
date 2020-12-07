using Becom.EDI.PersonalDataExchange.Model;
using Becom.EDI.PersonalDataExchange.Model.Enums;
using Becom.EDI.PersonalDataExchange.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecordings.Models;

namespace TimeRecordings.Components
{
    public partial class TimeRecordingsPresenceStatus
    {
        [Inject]
        public ILogger<TimeRecordingsPresenceStatus> Logger { get; set; }

        [Inject]
        public IZeiterfassungsService ZeiterfassungsService { get; set; }

        private List<TimeRecordingsPresenceStatusViewModel> employeeBaseInfos;

        public List<TimeRecordingsPresenceStatusViewModel> FoundEmployeesDisplay { get; set; } = new List<TimeRecordingsPresenceStatusViewModel>();

        private string searchText;
        public string SearchText
        {
            get
            {
                return searchText;
            }
            set
            {
                if (searchText != value)
                {
                    searchText = value;
                    InpChanged();
                }
            }
        }

        public string CheckedEmployeeText { get; set; } = string.Empty;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    var emps = await ZeiterfassungsService.GetEmployeeList(CompanyEnum.Austria);
                    employeeBaseInfos = emps.Select(x => new TimeRecordingsPresenceStatusViewModel { EmployeeId = x.EmployeeId, FirstName = x.FirstName, LastName = x.LastName, Type = PresenceType.UNKNOWN }).ToList();
                    FoundEmployeesDisplay = employeeBaseInfos;
                    StateHasChanged();
                }
            }
            catch(Exception ex)
            {
                Logger.LogError(ex.Message, ex);
            }
        }

        void InpChanged()
        {
            if (SearchText == null) return;
            FoundEmployeesDisplay = employeeBaseInfos.Where(x => string.Concat(x.FirstName, x.LastName).ToUpper().Contains(SearchText.ToUpper())).ToList();
        }

        async Task LineClicked(TimeRecordingsPresenceStatusViewModel emp)
        {
            try
            {
                var status = await ZeiterfassungsService.GetEmployeePresenceStatus(CompanyEnum.Austria, emp.EmployeeId);
                emp.Type = status.Type;
                CheckedEmployeeText = emp.Type switch
                {
                    PresenceType.UNKNOWN => string.Empty,
                    PresenceType.AN => $"Mitarbeiter {emp.FirstName} {emp.LastName} ist anwesend",
                    PresenceType.AB => $"Mitarbeiter {emp.FirstName} {emp.LastName} ist abwesend",
                    _ => string.Empty,
                };
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
            }
        }

        string GetCssClass(TimeRecordingsPresenceStatusViewModel emp)
        {
            if (emp.Type == PresenceType.AB)
            {
                return "trpr-ab";
            }
            else if (emp.Type == PresenceType.AN)
            {
                return "trpr-an";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
