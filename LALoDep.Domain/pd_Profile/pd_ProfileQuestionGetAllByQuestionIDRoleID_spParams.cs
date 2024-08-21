using System;

namespace LALoDep.Domain.PD_PDAction
{
    public class pd_ProfileQuestionGetAllByQuestionIDRoleID_spParams
    {
        public int ProfileQuestionID { get; set; }
        public int RoleID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}