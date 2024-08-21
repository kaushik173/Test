using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.Mobile
{
    
    public class pd_WorkGetByCaseID_spParams
    {
        public int  CaseID { get; set; }
        public int WorkerPersonID { get; set; }
        public int ClientPersonID { get; set; }
       
        public int UserID { get; set; }
        public Guid? BatchLogJobID { get; set; }
    }
    public class pd_WorkGetByCaseID_spResult
    {
        public int? CaseID { get; set; }
        public int? WorkID { get; set; }
        public int? PersonID { get; set; }
        public decimal? WorkHours { get; set; }
        public string Phase { get; set; }
        public string WorkDescriptionCodeMobileValue { get; set; }
        public string WorkStartDate { get; set; }
        public string WorkerFirstName { get; set; }
        public string WorkerLastName { get; set; }

        public int UserID { get; set; }
        public Guid? BatchLogJobID { get; set; }
    }
} 