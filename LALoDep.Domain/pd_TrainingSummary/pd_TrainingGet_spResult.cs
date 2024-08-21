using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_TrainingSummary
{
    public class pd_TrainingGet_spResult
    {
        public int TrainingID { get; set; }
        public int AgencyID { get; set; }
        public int PersonID { get; set; }
        public string TrainingCourseTitle { get; set; }
        public string TrainingProvider { get; set; }
        public string TrainingSubjectMatter { get; set; }
        public Int16 TrainingParticipatory { get; set; }
        public decimal TrainingHours { get; set; }
        public string TrainingCompletionDate { get; set; }
        public string TrainingCompletionDateTwo { get; set; }
        public int TrainingCreditTypeCodeID { get; set; }
        public Int16 RecordStateID{ get; set; }
        public int? TrainingVenueCodeID { get; set; }
        public int? TrainingIVeEligibleCodeID { get; set; }
    }
}
