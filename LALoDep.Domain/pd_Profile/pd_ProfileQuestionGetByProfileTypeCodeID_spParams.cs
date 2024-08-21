using System;

namespace LALoDep.Domain.pd_Profile
{
    public class pd_ProfileQuestionGetByProfileTypeCodeID_spParams
    {
        
         
        public int  ProfileTypeCodeID { get; set; }
        public int  AgencyID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}