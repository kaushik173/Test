using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
    public class pd_CodeHearingFindingOrderTypeGetByHearingTypeCodeID_spParams
    {
        public int HearingTypeCodeID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_CodeHearingFindingOrderTypeGetByHearingTypeCodeID_spResult
    {
        public int? CodeHearingFindingOrderID { get; set; }
        public int? HearingTypeCodeID { get; set; }
        public int? HearingFindingOrderCodeID { get; set; }
        public string HearingFindingOrderCodeValue { get; set; }
        public int? PRT { get; set; }
        public string CodeEnumName { get; set; }
        public string CodeMobileValue { get; set; }
    }
}
