
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.TrainingImport
{

    public class TrainingImport_StageRecord_spParams
    {
        public string FileName { get; set; }
        public string JcatsPersonID { get; set; }
        public string CourseTitle { get; set; }
        public string Provider { get; set; }
        public string SubjectMatter { get; set; }
        public string JcatsCreditTypeCodeID { get; set; }
        public string Participatory { get; set; }
        public string Hours { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string JcatsVenueCodeID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }

}