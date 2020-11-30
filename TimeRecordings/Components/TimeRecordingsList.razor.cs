﻿using Becom.EDI.PersonalDataExchange.Model;
using Becom.EDI.PersonalDataExchange.Model.Enums;
using Becom.EDI.PersonalDataExchange.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimeRecordings.Components
{
    public partial class TimeRecordingsList
    {
        [Inject]
        public IZeiterfassungsService ZeiterfassungsService { get; set; }

        [CascadingParameter]
        public CompanyEnum Company { get; set; }

        [CascadingParameter]
        public int EmployeeId { get; set; }

        private DateTime from = DateTime.Now;
        public DateTime From
        {
            get
            {
                return from;
            }
            set
            {
                if (from != value)
                {
                    from = value;
                    _ = update();
                }
            }
        }

        private DateTime to = DateTime.Now;
        public DateTime To
        {
            get
            {
                return to;
            }
            set
            {
                if (to != value)
                {
                    to = value;
                    _ = update();
                }
            }
        }

        public List<EmployeeTimeDetail> Details { get; set; } = null;

        protected override async Task OnParametersSetAsync()
        {
            To = DateTime.Now;
            From = To.AddMonths(-1);

            await update(true);
        }

        protected override void OnInitialized()
        {
            To = DateTime.Now;
            From = To.AddMonths(-1);
        }

        private async Task update(bool firstTime=false)
        {
            try
            {
                //Details = await ZeiterfassungsService.GetEmployeeTimeDetails(Company, EmployeeId, From, To);

                if (!firstTime) StateHasChanged();
            }
            catch (Exception)
            {
                Details = null;
            }
        }
    }
}
