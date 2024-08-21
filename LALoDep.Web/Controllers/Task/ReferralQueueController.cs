using System;
using System.Linq;
using System.Web.Mvc;
using LALoDep.Models.Task;
using LALoDep.Custom;
using LALoDep.Domain.ref_Referral;
using LALoDep.Core.Custom.Extensions;
using DataTables.Mvc;
using LALoDep.Domain.pd_Person;

namespace LALoDep.Controllers
{
    public partial class TaskController
    {
        //[ClaimsAuthorize(IsCasePage = false, CustomSecurityItemIds = PageLevelSecurityItemIds.MyARQueuePage, PageSecurityItemID = SecurityToken.ViewActionRequest)]
        public virtual ActionResult ReferralQueue(string id)
        {
            var dateRangeType = UtilityService.ExecStoredProcedureWithResults<ref_ReferralQueueDateRangeTypes_spResult>("ref_ReferralQueueDateRangeTypes_sp",
                     new ref_ReferralQueueDateRangeTypes_spParams()
                     {
                         BatchLogJobID = Guid.NewGuid(),
                         UserID = UserManager.UserExtended.UserID,
                     });

            var viewModel = new ReferralQueueViewModel
            {

                DateRangeTypes = dateRangeType.Select(x => new SelectListItem { Text = x.CodeDisplay, Value = x.CodeID.ToString() }),
                DateRangeTypeCodeID = dateRangeType.FirstOrDefault(x => x.Selected == 1)?.CodeID
            };

            viewModel.ReferralPersonID = UserManager.UserExtended.PersonID;
            viewModel.PersonName = UserManager.UserExtended.FullName;
            if (!string.IsNullOrEmpty(id))
            {
                var PersonInfo = UtilityService.ExecStoredProcedureWithResults<pd_PersonGet_spResult>("pd_PersonGet_sp", new pd_PersonGet_spParams()
                {
                    PersonID = id.ToDecrypt().ToInt(),
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                }).FirstOrDefault();
                if (PersonInfo != null)
                {
                    viewModel.ReferralPersonID = PersonInfo.PersonID;
                    viewModel.PersonName = PersonInfo.LastName + ", " + PersonInfo.FirstName;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult ReferralQueueSearch(ReferralQueueViewModel viewModel)
        {
            var list = UtilityService.ExecStoredProcedureWithResults<ref_ReferralQueue_spResult>("ref_ReferralQueue_sp",
                     new ref_ReferralQueue_spParams()
                     {
                         PersonID = viewModel.ReferralPersonID,
                         StartDate = viewModel.StartDate.ToDateTimeValue(),
                         EndDate = viewModel.EndDate.ToDateTimeValue(),
                         DateRangeTypeCodeID = viewModel.DateRangeTypeCodeID,
                         BatchLogJobID = Guid.NewGuid(),
                         UserID = UserManager.UserExtended.UserID,
                         IncludeCompletedReferrals = viewModel.IncludeCompletedReferrals ? 1 : 0
                     }).ToList().Select(o => new
                     {

                         EncryptedReferralID = o.ReferralID.ToEncrypt(),
                         o.AgencyID,
                         o.CaseID,
                         o.Client,
                         o.CompleteDate,
                         o.CurrentAttorney,
                         o.DueDate,
                         o.Eligibity,
                         o.NG_NavigationURL,
                         o.ReferralType,
                         o.RequestDate,
                         o.RequestedFor,
                         o.SortDate,
                         o.ReferralID,
                         o.SubmittedCaseNbr


                     }).ToList();

            var jsonResult = Json(new DataTablesResponse(0, list, list.Count, list.Count));
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }



        [HttpPost]
        public virtual ActionResult ReferralQueuePrint(ReferralQueueViewModel viewModel)
        {
            var dtReferralQueuePrint = UtilityService.ExecStoredProcedureForDataTable("ref_ReferralQueuePrint_sp",
                     new ref_ReferralQueuePrint_spParams()
                     {
                         PersonID = viewModel.ReferralPersonID,
                         StartDate = viewModel.StartDate.ToDateTimeValue(),
                         EndDate = viewModel.EndDate.ToDateTimeValue(),
                         DateRangeTypeCodeID = viewModel.DateRangeTypeCodeID,
                         BatchLogJobID = Guid.NewGuid(),
                         UserID = UserManager.UserExtended.UserID,
                         IncludeCompletedReferrals = viewModel.IncludeCompletedReferrals ? 1 : 0
                     });
            dtReferralQueuePrint.TableName = "Referral Queues";
            var fileName = "ReferralQueue_" + Guid.NewGuid().ToString() + ".xlsx";
            var generatedFilePath = UtilityFunctions.GetDocumentDownloadFolderPath() + fileName;

            UtilityFunctions.ExportDataTableToExcel(dtReferralQueuePrint, generatedFilePath);

            
            return new LALoDep.Custom.Actions.DownloadActionResult(fileName);
        }

            
    }
}