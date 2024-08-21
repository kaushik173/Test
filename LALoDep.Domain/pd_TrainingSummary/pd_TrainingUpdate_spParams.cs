using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_TrainingSummary
{
    public class pd_TrainingUpdate_spParams
    {
        public int TrainingID { get; set; }
        public int? AgencyID { get; set; }
        public int PersonID { get; set; }
        public string TrainingCourseTitle { get; set; }
        public string TrainingProvider { get; set; }
        public string TrainingSubjectMatter { get; set; }
        public int? TrainingCreditTypeCodeID { get; set; }
        public int TrainingParticipatory { get; set; }
        public decimal? TrainingHours { get; set; }
        public string TrainingCompletionDate { get; set; }
        public string TrainingCompletionDateTwo { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int?  TrainingVenueCodeID { get; set; }
        public int?   TrainingIVeEligibleCodeID { get; set; }
    }
}
