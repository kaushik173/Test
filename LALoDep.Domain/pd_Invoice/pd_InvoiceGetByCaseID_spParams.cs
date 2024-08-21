using System;

namespace LALoDep.Domain.pd_Invoice
{

    public class pd_RegenerateInvoice_spParams
    {
        public int InvoiceID
        { get; set; }
      


        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }





    }


    public class pd_InvoiceVoidByInvoiceID_spParams
    {
        public int InvoiceID
        { get; set; }
        public string RecordTimeStamp
        { get; set; }
         

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }





    }

    
    public class pd_InvoiceDelete_spParams
    {
        public int ID
        { get; set; }
        public string RecordTimeStamp
        { get; set; }
        public string LoadOption
        { get; set; }
        public int RecordStateID
        { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }





    }
    public class pd_InvoiceGetByCaseID_spParams
    {
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_InvoiceGetByCaseID_spResult
    {
        public int InvoiceID { get; set; }
        public string InvoiceDate { get; set; }
        public string Status { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public DateTime? InvoiceDateTime { get; set; }
        public int InvoiceStatusCodeID { get; set; }
        public string OtherParties { get; set; }
        public int Void { get; set; }
        public string NoteEntry { get; set; }
        public int? NoteID { get; set; }

    }
}
