
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace LALoDep.Domain.TitleIVe
{

    public class TitleIVeStaffInsertUpdate_spParams
    {
        public int? ErrorID { get; set; }
        public int? UserID { get; set; }
        public int? PersonID { get; set; }
        public string StaffTitle { get; set; }
        public decimal? PercentBenefitsCAC { get; set; }
        public decimal? PercentBenefitsFFDRP { get; set; }
        public decimal? MonthlySalaryAndBenefits { get; set; }
        public decimal? TotalContractPayments { get; set; }
        public bool? FullTime { get; set; }
        public string AlternateWorkSchedule { get; set; }
        public string NormalWorkHours { get; set; }
        public bool? NeedsActivityLog { get; set; }
        public bool? IsEmployee { get; set; }
        public int? OHCodeID { get; set; }
    }

}