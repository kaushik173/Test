using System;

namespace LALoDep.Core.NG_sp.com_Navigation
{
    public class NG_com_NavigationGetByCaseIDTaskID_spParams
    {
        public int CaseID { get; set; }
        public int NavigationTaskID { get; set; }
        public int NavigationID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
