using System;

namespace LALoDep.Domain.pd_Profile
{
    public class pd_ProfileGet_spParams
    {
       
        public int  ProfileID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}