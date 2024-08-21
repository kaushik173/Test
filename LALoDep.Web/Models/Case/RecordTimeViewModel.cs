using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_Hearing;

namespace LALoDep.Models.Case
{
    public class RecordTimeViewModel
    {
        public List<pd_RoleGetByCaseIDWorkerCriteria_spResult> Workers { get; set; }
        public List<pd_RoleGetByCaseIDClientCriteria_spResult> Clients { get; set; }
        public int WorkerPersonID { get; set; }
        public int ClientPersonID{ get; set; }
        public int HearingID { get; set; }
        public RecordTimeViewModel()
        {
            Workers = new List<pd_RoleGetByCaseIDWorkerCriteria_spResult>();
            Clients = new List<pd_RoleGetByCaseIDClientCriteria_spResult>();
        }
    }
   
}