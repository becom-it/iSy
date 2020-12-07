using Microsoft.AspNetCore.Components;
using System;
using iSy.Shared.Extensions;
using System.Collections.Generic;

namespace iSy.Controls.Components
{
    public partial class Calendar
    {
        [Parameter]
        public int Month { get; set; }

        [Parameter]
        public int Year { get; set; }

        public List<WeekInfo> Weeks { get; set; } = new List<WeekInfo>();

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public DateTime Current { get; set; }

        protected override void OnInitialized()
        {
            Start = new DateTime(Year, Month, 1);
            while(Start.DayOfWeek != DayOfWeek.Monday)
            {
                Start = Start.AddDays(-1);
            }

            End = new DateTime(Year, Month, 1).LastDayOfMonth();
            while(End.DayOfWeek != DayOfWeek.Sunday)
            {
                End = End.AddDays(1);
            }

            Current = Start;

            Weeks = new List<WeekInfo>();
            while (Start <= End)
            {
                var current = Start;
                var week = new WeekInfo();
                while(current.DayOfWeek != DayOfWeek.Sunday)
                {
                    var d = new DayInfo { Day = current };
                    week.Days.Add(d);
                    current = current.AddDays(1);
                }

                Weeks.Add(week);
                Start = Start.AddDays(7);
            }
        }
    }

    public record WeekInfo
    {
        public List<DayInfo> Days { get; set; } = new List<DayInfo>();
    }

    public record DayInfo
    {
        public DateTime Day { get; set; }
    }
}
