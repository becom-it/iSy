using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeData.Models;

namespace PeopleFinder.Services
{
    public class PeopleFinderState
    {
        public event Func<LdapEmployee, Task> OnEmployeeSearched;
        public event Func<LdapEmployee, Task> OnEmployeeClicked;

        public async Task EmployeeSearched(LdapEmployee employee)
        {
            if(OnEmployeeSearched != null)
            {
                await OnEmployeeSearched.Invoke(employee);
            }
        }

        public async Task EmployeeClicked(LdapEmployee employee)
        {
            if(OnEmployeeClicked != null)
            {
                await OnEmployeeClicked.Invoke(employee);
            }
        }
    }
}
