using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.qcal
{
    public class qcal_QuickCaseAddGetDefaults_spParams {
  public int? AgencyID { get; set; }
  public int? AttorneyPersonID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class qcal_QuickCaseAddGetDefaults_spResult
	{
        public byte? DOBRequiredForChildren { get; set; }
        public int? CheckParent1Flag	{ get; set; }
		public int? CheckChild1Flag	{ get; set; }
		public System.Nullable<System.DateTime> ApptDate	{ get; set; }
		public int? CaseDepartmentCodeID	{ get; set; }
		public int? CaseJudgePersonID	{ get; set; }
		public System.Nullable<System.DateTime> PetitionFileDate	{ get; set; }
		public System.Nullable<System.DateTime> HearingDate	{ get; set; }
		public int? HearingTypeCodeID	{ get; set; }
		public int? AttorneyPersonID	{ get; set; }
        public int? PetitionTypeCodeID { get; set; }
    }
}
