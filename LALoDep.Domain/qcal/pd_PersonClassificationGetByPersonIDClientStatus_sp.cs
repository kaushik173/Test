namespace LALoDep.Domain.qcal
{
    public class pd_PersonClassificationGetByPersonIDClientStatus_spParams
    {
        public int? PersonID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class pd_PersonClassificationGetByPersonIDClientStatus_spResult
    {
      
        public int? PersonClassificationID { get; set; }
        public int? PersonID { get; set; }
        public int? PersonClassificationCodeID { get; set; }
        public string PersonClassificationStartDate { get; set; }
        public string PersonClassificationEndDate { get; set; }
        public int? PersonClassificationEndReasonCodeID { get; set; }
        public short? RecordStateID { get; set; }
        public decimal RecordTimeStamp { get; set; }
        public string PersonClassification { get; set; }
        public string PersonClassificationEndReason { get; set; }
        public System.Nullable<System.DateTime> SortDate { get; set; }
        public int? CanEditFlag { get; set; }
    }
}
