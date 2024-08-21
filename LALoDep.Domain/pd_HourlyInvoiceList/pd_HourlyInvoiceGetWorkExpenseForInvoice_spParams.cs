using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_HourlyInvoiceList
{
    public class pd_HourlyInvoiceGetWorkExpenseForInvoice_spParams
    {
        public int PersonID { get; set; }
        public int AgencyCountyID { get; set; }
        public string SortOption { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public string LoadOption { get; set; }
    }

    public class pd_HourlyInvoiceGetWorkExpenseForInvoice_spResult
    {
        public int ID { get; set; }
        public Guid? BatchGUID { get; set; }
        public int? PersonID { get; set; }
        public string PersonFullName { get; set; }
        public string HourlyInvoiceDate { get; set; }
        public string CaseName { get; set; }
        public string Description { get; set; }
        public decimal? Hours { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Amount { get; set; }
        public int? AgencyID { get; set; }
        public int? AgencyCountyID { get; set; }
        public int? WorkID { get; set; }
        public int? HourlyExpenseID { get; set; }
        public string RecordType { get; set; }
    }
}
