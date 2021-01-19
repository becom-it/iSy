using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeData.Models;
using Microsoft.AspNetCore.Components;
using PeopleFinder.Services;

namespace PeopleFinder.Components
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "<Pending>")]
    public partial class OrganigramTree : IDisposable
    {
        [Inject]
        public PeopleFinderState State { get; set; }

        [Inject]
        EmployeeData.Services.IEmployeeService EmpService { get; set; }

        public bool Ready { get; set; } = false;

        public LdapEmployee Manager { get; set; }

        public LdapEmployee TeamLead { get; set; }

        public List<LdapEmployee> Employees { get; set; }

        protected override void OnInitialized()
        {
            State.OnEmployeeSearched += State_OnEmployeeSearched;
        }

        private async Task State_OnEmployeeSearched(LdapEmployee arg)
        {
            Ready = false;

            var currEmp = await EmpService.LoadEmployeeWithId(arg.Account);

            if (currEmp.DirectReports.Count > 0)
            {
                //TeamLead
                setTeamLead(currEmp);
            }
            else
            {
                //Employee;
                setTeamLead(currEmp.Manager);
                TeamLead.IsCurrent = false;
                var curr = Employees.Where(x => x.EmployeeId == currEmp.EmployeeId).FirstOrDefault();
                if (curr != null)
                {
                    curr.IsCurrent = true;
                }
            }

            Ready = true;
            StateHasChanged();
        }

        private void setTeamLead(LdapEmployee teamLead)
        {
            if (teamLead == TeamLead) return;
            TeamLead = teamLead;
            TeamLead.IsCurrent = true;
            Employees = TeamLead.DirectReports;
            Manager = TeamLead.Manager;
        }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        {
            State.OnEmployeeSearched -= State_OnEmployeeSearched;
        }
    }
}
