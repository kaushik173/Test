using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.sup_Case
{
    public class sup_CasePetitionNumberSet_spParams
    {
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid? BatchLogJobID { get; set; }
        public int? AdminFlag { get; set; }
    }
    public class sup_SetAttorneyFlags_spParams
    {
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid? BatchLogJobID { get; set; }
        public int? AdminFlag { get; set; }
    }
    public class sup_CaseNameSet_spParams
    {
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid? BatchLogJobID { get; set; }
        
    }
    
}
