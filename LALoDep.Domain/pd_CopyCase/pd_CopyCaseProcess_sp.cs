
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.pd_CopyCase
{

    public class pd_CopyCaseProcess_spParams {
  public int? FromCaseID { get; set; }
  public int? ToAgencyID { get; set; }
  public int? ToAttorneyPersonID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }
  public int? IncludeRoleID { get; set; }
  public byte? CopyAllFlag { get; set; }
  public DateTime? TransferDate { get; set; }
  public byte? DoNotRunSummaryFlag { get; set; }
  public string IncludeClientPersonIDList { get; set; }
  public string ExcludeClientPersonIDList { get; set; }

}

}