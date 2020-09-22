using System;
using System.Collections.Generic;
using System.Text;

namespace iSy.Shared.Models.Config
{
    public class Weather
    {
        public string AppId { get; set; }
        public List<Company> Companies { get; set; }
    }

    public class Company
    {
        public int MapId { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string CID { get; set; }
        public string Country { get; set; }
        public int Lat { get; set; }
        public int Lng { get; set; }
        public string TimeZone { get; set; }
    }
}
