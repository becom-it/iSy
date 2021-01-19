using System;
using System.Threading.Tasks;
using EmployeeData.Models;
using Microsoft.AspNetCore.Components;
using PeopleFinder.Services;

namespace PeopleFinder.Components
{

    public partial class EmployeeBox : IDisposable
    {
        [Inject]
        public PeopleFinderState State { get; set; }

        [Parameter]
        public LdapEmployee Employee { get; set; } = null;

        async Task empClicked()
        {
            if (Employee.IsCurrent) return;
            await State.EmployeeClicked(Employee);
        }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        {
            //State.OnEmployeeSearched -= State_OnEmployeeSearched;
        }
    }
}
