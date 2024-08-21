using LALoDep.Domain.IVeActivity;
using LALoDep.Domain.TitleIVe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.IVEActvityLog
{
    public class IVEActvityLogViewModel
    {
        public IVEActvityLogViewModel()
        {
            AgencyCountyList = new List<SelectListItem>();

        }
        public int? ActivityLogID { get; set; }
        public string PersonName { get; set; }
        public int? PersonID { get; set; }
        public string Title { get; set; }
        public int? AgencyCountyID { get; set; }
        public IEnumerable<SelectListItem> AgencyCountyList { get; set; }
        public DateTime ActivityMonth { get; set; }

        public string EmployeeName { get; set; }
        public string SupervisorSignedName { get; set; }
        public string DateSignedEmployee { get; set; }
        public string DateSignedSupervisor { get; set; }
        public int? UseWorkHoursForActivityLog { get; set; }
        public int? OKToSwitchToDifferentEmployee { get; set; }
        public string SaveSignatureNotAllowedMessage { get; set; }
    }

    public class IVEActvitySheetViewModel
    {
        public List<IVeActivityLogDetail> IVeActivityLogDetails { get; set; }
        public int? ActivityLogID { get; set; }
        public DateTime ActivityMonth { get; set; }
        public decimal? ParentActivityLogDetailID { get; set; }
        public decimal? UseWorkHoursForActivityLog { get; set; }
    }

    public class IVEActvityLogExecViewModel
    {
        public IVEActvityLogExecViewModel()
        {
          

        }
        public int? ActivityLogID { get; set; }
        public string PersonName { get; set; }
        public int? PersonID { get; set; }
        public string Title { get; set; }
        public int? AgencyCountyID { get; set; }

        public DateTime ActivityMonth { get; set; }

        public string EmployeeName { get; set; }
        public string SupervisorSignedName { get; set; }
        public string DateSignedEmployee { get; set; }
        public string DateSignedSupervisor { get; set; }

        public decimal? NonDependPercent_ExecDir { get; set; }
        public decimal? TimeOffPercent_ExecDir { get; set; }
        public decimal? PTOReimbursmentRate_ExecDir { get; set; }
        public decimal? FFDRPWorked { get; set; }
        public decimal? FFDRPPaidTimeOff { get; set; }
        public decimal? TotalFFDRPEligible { get; set; }
        public decimal? TotalFFDRPIneligible { get; set; }
        public List<TitleIVeExecDirCountyAllocationGet_spResult> TitleIVeExecDirCountyAllocationList { get; set; }

        

    }
}