
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.TitleIVe
{

    public class TitleIVeProfessionalServiceDelete_spParams
    {
        public int? ErrorID { get; set; }
        public int? TitleIVeProfessionalServiceID { get; set; }
        public int? UserID { get; set; }

    }
    public class TitleIVeProfessionalServiceDelete_spResult
    { 
        public string DocumentName { get; set; }
      

    }
    public class TitleIVeOperatingExpenseDelete_spParams
    {
        public int? ErrorID { get; set; }
        public int? TitleIVeOperatingExpenseID { get; set; }
        public int? UserID { get; set; }

    }
    public class TitleIVeOperatingExpenseDelete_spResult
    {
        public string DocumentName { get; set; }


    }
    public class TitleIVeTravelExpenseDelete_spParams
    {
        public int? ErrorID { get; set; }
        public int? TitleIVeTravelID { get; set; }
        public int? UserID { get; set; }

    }
    public class TitleIVeTravelExpenseDelete_spResult
    {
        public string DocumentName { get; set; }


    }

}