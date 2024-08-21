using LALoDep.Domain.CSEC;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Case
{
    public class CSECEditViewModel
    {
        public int CSECID { get; set; }
        public int CaseID { get; set; }
        public string Child { get; set; }
        public string DueDate { get; set; }
        public string CompletionDate { get; set; }
        public string AssignedTo { get; set; }
        public string CSECNote { get; set; }

        public bool CSECNoteChanged { get; set; }
        public Dictionary<string, short?> Answers { get; set; }
        public List<CSECGetQuestions_spResult> Questions { get; set; }

        public int buttonId { get; set; }

        public CSECEditViewModel()
        {
            Questions = new List<CSECGetQuestions_spResult>();
            Answers = new Dictionary<string, short?>();

        }
    }
}