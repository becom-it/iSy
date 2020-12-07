using Microsoft.AspNetCore.Components;

namespace TimeRecordings.Components
{
    public partial class TimeRecordingsCalendar
    {

        [CascadingParameter]
        public int EmployeeId { get; set; }
    }
}