using System.Collections.Generic;
using System.Web.Mvc;
using LALoDep.Domain.pd_Code;

namespace LALoDep.Models.CaseOpening
{
    public class NotesModel
    {

        public int NoteTypeID { get; set; }
        public string Subject { get; set; }
        public string Notes { get; set; }
        public IEnumerable<SelectListItem> NoteTypeList { get; set; }

        public IEnumerable<SelectListItem> BroadcastNotesPages { get; set; }

        public string SelectedPageIds { get; set; }
      

        public NotesModel()
        {
            NoteTypeList = new List<SelectListItem>();
            BroadcastNotesPages = new List<SelectListItem>();
        }


    }
}