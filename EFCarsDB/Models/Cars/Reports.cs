using System;

namespace EFCarsDB.Models
{
    public partial class Reports
    {
        public int Id { get; set; }
        public string ReportData { get; set; }
        public DateTime RequestedDateTime { get; set; }
        public bool AddedToDatabase { get; set; }

    }
}
