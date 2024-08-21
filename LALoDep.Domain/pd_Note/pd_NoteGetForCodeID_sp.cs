namespace LALoDep.Domain.pd_Note

{
    public class pd_NoteGetForCodeID_spParams
    {
        public int? CodeID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class pd_NoteGetForCodeID_spResult
    {
        
        public int? NoteID { get; set; }
        public int? AgencyID { get; set; }
        public int? NoteEntityCodeID { get; set; }
        public int? NoteEntityTypeCodeID { get; set; }
        public int? EntityPrimaryKeyID { get; set; }
        public int? NoteTypeCodeID { get; set; }
        public string NoteSubject { get; set; }
        public string NoteEntry { get; set; }
        public int? CaseID { get; set; }
        public int? PetitionID { get; set; }
        public int? HearingID { get; set; }
        public short? RecordStateID { get; set; }
        public byte[] RecordTimeStamp { get; set; }
        public string AgencyDisplay { get; set; }
        public string TypeDisplay { get; set; }
        public string NoteHeaderDisplay { get; set; }
        public string SortDate { get; set; }
        public int? CanEditFlag { get; set; }
        public int? CanDeleteFlag { get; set; }
    }
}
