using System;

namespace LALoDep.Domain.PD_PDAction
{
    public class pd_ProfileAnswerGetByProfileQuestionID_spParams
    {
        public int ProfileQuestionID { get; set; }
     
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}