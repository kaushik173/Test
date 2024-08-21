public class pd_ProfileGet_spResult
{
    public int ProfileID { get; set; }
    public int? AgencyID { get; set; }
    public int? RoleID { get; set; }
    public int? ProfileQuestionID { get; set; }
    public int? ProfileAnswerID { get; set; }
    public string ProfileFreeformAnswer { get; set; }
    public System.DateTime? ProfileDate { get; set; }
    public int? HearingReportFilingDueID { get; set; }
    public short RecordStateID { get; set; }
        
    public int? NoteID { get; set; }
    public int? NoteAgencyID { get; set; }
    public int? NoteEntityCodeID { get; set; }
    public int? NoteEntityTypeCodeID { get; set; }
    public int? EntityPrimaryKeyID { get; set; }
    public int? NoteTypeCodeID { get; set; }
    public string NoteSubject { get; set; }
    public string NoteEntry { get; set; }
    public int? CaseID { get; set; }
    public int? PetitionID { get; set; }
    public int? HearingID { get; set; }
    public short? NoteRecordStateID { get; set; }
      
}