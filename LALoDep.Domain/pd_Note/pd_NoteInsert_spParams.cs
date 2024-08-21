using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Note
{
    public class pd_NoteInsert_spParams
    {
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int? AgencyID { get; set; }
        public int NoteEntitySystemValueTypeID { get; set; }
        public int NoteEntityTypeSystemValueTypeID { get; set; }
        public int EntityPrimaryKeyID { get; set; }
        public int NoteTypeCodeID { get; set; }
        public string NoteSubject { get; set; }
        public string NoteEntry { get; set; }
        public int CaseID { get; set; }
        public int? PetitionID { get; set; }

        public int? HearingID { get; set; }

        public int RecordStateID { get; set; }

        public string RecordTimeStamp { get; set; }
    }
    public class pd_NoteInsertForCode_spParams
    {
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int? AgencyID { get; set; }
        public int NoteEntitySystemValueTypeID { get; set; }
        public int NoteEntityTypeSystemValueTypeID { get; set; }
        public int EntityPrimaryKeyID { get; set; }
        public int NoteTypeCodeID { get; set; }
        public string NoteSubject { get; set; }
        public string NoteEntry { get; set; }
        public int? CaseID { get; set; }
        public int? PetitionID { get; set; }

        public int? HearingID { get; set; }

        public int RecordStateID { get; set; }

        public string RecordTimeStamp { get; set; }
    }
    public class del_NoteUpdate_spParams
    {
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int? NoteID { get; set; }
        public int? AgencyID { get; set; }
        public int? NoteEntityCodeID { get; set; }
        public int? NoteEntityTypeCodeID { get; set; }
        public int EntityPrimaryKeyID { get; set; }
        public int NoteTypeCodeID { get; set; }
        public string NoteSubject { get; set; }
        public string NoteEntry { get; set; }
        public int CaseID { get; set; }
        public int? PetitionID { get; set; }

        public int? HearingID { get; set; }

        public int RecordStateID { get; set; }

        public string RecordTimeStamp { get; set; }
    }
    public class pd_NotePanelInsert_spParams
    {
        public int NotePanelID { get; set; }
        public int AgencyID { get; set; }
        public int NoteID { get; set; }
        public int NotePanelCodeID { get; set; }
        public int RecordStateID { get; set; }
        public decimal? RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_NotePanelDelete_spParams
    {
        public int ID { get; set; }
        public int RecordStateID { get; set; }
        public decimal? RecordTimeStamp { get; set; }
        public string LoadOption{ get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    
}

