using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_TrainingSummary
{
    public class pd_TrainingGetByPersonIDStartDateEndDate_spResult
    {
        public int TrainingID { get; set; }
        public int AgencyID { get; set; }
        public int PersonID { get; set; }
        public string TrainingCourseTitle { get; set; }
        public string TrainingProvider { get; set; }
        public string TrainingSubjectMatter { get; set; }
        public int TrainingCreditTypeCodeID { get; set; }
        public Int16 TrainingParticipatory { get; set; }
        public decimal TrainingHours { get; set; }
        public DateTime? TrainingCompletionDate { get; set; }
        public DateTime? TrainingCompletionDateTwo { get; set; }
        public Int16 RecordStateID { get; set; }
        public string TrainingCreditTypeCodeValue { get; set; }
        public string SortDate { get; set; }
        public string CreditType { get; set; }
        public string Venue { get; set; }
    }
}
