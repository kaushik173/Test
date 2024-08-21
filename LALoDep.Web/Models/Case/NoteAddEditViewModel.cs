using LALoDep.Domain.pd_Note;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Models.Case
{
    public class NoteAddEditViewModel
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
        public Int16 RecordStateID { get; set; }
        public bool CanAddNote { get; set; }
        public bool CanEditNote { get; set; }
        public int? CanEditFlag { get; set; }
        
              public string ControlType { get; set; }
        public int? CanEditSubjectFlag { get; set; }
        public int? HideNoteTypeFlag { get; set; }
        public int? HideBroadcastNotesFlag { get; set; }
        public int? HideClientsAttachedFlag { get; set; }
        public List<PanelList> PanelList { get; set; }
        public List<NoteTypeList> NoteTypeList { get; set; }

        public List<NotePersonGetAll_spResult> NotePersonList { get; set; }

        public List<NoteClientList> NoteClientListForAddEdit { get; set; }
        
    }

    public class PanelList
    {
        public int CodeID { get; set; }
        public string Type { get; set; }
        public int? NotePanelKey { get; set; }
        public int Selected { get; set; }
        public int IsCurrentSelected{ get; set; }
    }
    public class NoteClientList
    {
        public int PersonID { get; set; }
       
        public int  NotePersonID { get; set; }
        public int Selected { get; set; }
        
    }
    public class NoteTypeList
    {
        public string NoteTypeDisplay { get; set; }
        public int? CodeID { get; set; }
        public string CodeValue { get; set; }
        public string CodeShortValue { get; set; }
        public int? CodeTypeID { get; set; }
    }

}
