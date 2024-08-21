using System;

namespace LALoDep.Domain.NgInvoice
{
    public class NgInvoice_GetContractors_spParams
    {
        public int? PersonID { get; set; }
        public DateTime? CurrentDate { get; set; }
        public string LoadOption { get; set; }
        public int? UserID { get; set; }

    }


    public class NgInvoice_GetContractors_spResult
    {
        public NgInvoice_GetContractors_spResult()
        {
        }
        public string PersonDisplay { get; set; }
        public int? PersonID { get; set; }
    }
}
