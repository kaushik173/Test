using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_MonthlyInvoiceList
{
    public class pd_MonthlyInvoiceDetailGetByMonthlyInvoiceID_spParams
    {
        public string LoadOption         {get;set;}
        public int? InvoiceMonthlyID   {get;set;}
        public int UserID             {get;set;}
        public Guid BatchLogJobID      {get;set;}
        public DateTime? AsOfDate           {get;set;}
        public int? AttorneyPersonID { get; set; }

      
    }

    public class pd_MonthlyInvoiceDetailGetByMonthlyInvoiceID_spResult
    {
        public int? SubmitYear { get; set; }
        public int? SubmitMonth { get; set; }
        public string SubmitMonthName { get; set; }
        public int? AlreadySubmittedFlag { get; set; }
        public string Client { get; set; }
        public int? ClientPersonID { get; set; }
        public string PetitionDocketDisplay { get; set; }
        public string NextCourtDisplay { get; set; }
        public string ClientDIsplay { get; set; }
        public string CaseNumber { get; set; }
        public int? PetitionID { get; set; }
        public int? NextHearingID { get; set; }
        public int? CaseID { get; set; }
        public DateTime? CaseAppointmentDate { get; set; }
        public DateTime? PetitionFileDateMIN { get; set; }
        public DateTime? PetitionCloseDateMAX { get; set; }
        public string ClientRoleType { get; set; }

        public string NotYetIncludedHeaderDisplay { get; set; }
        public string PrevioulyIncludedHeaderDisplay { get; set; }

        public string DetailsHeaderDisplay { get; set; }
    }
}
