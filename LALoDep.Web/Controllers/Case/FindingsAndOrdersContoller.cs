using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Models.Case;
using LALoDep.Domain.pd_Conflict;

namespace LALoDep.Controllers
{
    public partial class CaseController
    {
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.FindingsAndOrders, PageSecurityItemID = SecurityToken.ViewFindingsAndOrders)]
        public virtual ActionResult FindingsAndOrders(string id)
        {

            int hearingId = id.ToDecrypt().ToInt();
            ViewBag.HearingID = hearingId;
            var findings = UtilityService.ExecStoredProcedureWithResults<pd_HearingFindingOrderGetByHearingID_spResult>("pd_HearingFindingOrderGetByHearingID_sp",
                            new pd_HearingFindingOrderGetByHearingID_spParams { HearingID = hearingId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).ToList();

            if (!findings.Any() && UserManager.IsUserAccessTo(SecurityToken.AddFindingsAndOrders))
                return RedirectToAction("AddFindingsAndOrders", "Case", new { id = id, main = true });

            var viewModel = new FindingsAndOrdersListViewModel();

            var person = UtilityService.ExecStoredProcedureWithResults<pd_HearingFindingOrderPersonGetByHearingIDHearing_spResult>("pd_HearingFindingOrderPersonGetByHearingIDHearing_sp",
                            new pd_HearingFindingOrderPersonGetByHearingIDHearing_spParams { HearingID = hearingId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).ToList();

            var notices = UtilityService.ExecStoredProcedureWithResults<pd_HearingFindingOrderNoticeGetByHearingID_spResult>("pd_HearingFindingOrderNoticeGetByHearingID_sp",
                            new pd_HearingFindingOrderNoticeGetByHearingID_spParams { HearingID = hearingId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).ToList();

            viewModel.FindingAndOrderList = findings.Select(x => new FindingAndOrderListModel
            {HearingFindingOrderComment=x.HearingFindingOrderComment,
                HearingFindingOrderID = x.HearingFindingOrderID.ToEncrypt(),
                HearingFindingOrderCodeValue = x.HearingFindingOrderCodeValue,
                Person = string.Join(", ", person.Where(p => p.HearingFindingOrderID == x.HearingFindingOrderID).Select(p => p.PersonNameFirst + " " + p.PersonNameLast)),
                Notices = string.Join(", ", notices.Where(n => n.HearingFindingOrderID == x.HearingFindingOrderID).Select(n => n.Notice))
            }).ToList();

            var noteEntry = UtilityFunctions.NoteGetByEntity(hearingId, 113, 124).FirstOrDefault();
            if (noteEntry != null)
                viewModel.NoteEntry = noteEntry.NoteEntry;

            var hearingInfo = UtilityService.ExecStoredProcedureWithResults<pd_HearingGet_spResult>("pd_HearingGet_sp",
                            new pd_HearingGet_spParams() { HearingID = hearingId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).FirstOrDefault();

            if (hearingInfo != null)
            {
                viewModel.CaseID = hearingInfo.CaseID;
                viewModel.HearingID = hearingInfo.HearingID;
                viewModel.HearingType = hearingInfo.HearingType;
                viewModel.HearingDateTime = hearingInfo.HearingDateTime.ToDefaultFormat();
                viewModel.HearingJudge = hearingInfo.HearingJudge;
                viewModel.HearingDept = hearingInfo.HearingDept;
            }

            return View(viewModel);
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.FindingsAndOrders, PageSecurityItemID = SecurityToken.DeleteFindingsAndOrders)]
        public virtual ActionResult DeleteFindingsAndOrders(string id)
        {
            var findingID = id.ToDecrypt().ToInt();
            UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingFindingOrderDelete_sp",
                            new pd_HearingFindingOrderDelete_spParams() { ID = findingID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).FirstOrDefault();

            return Json(new { isSuccess = true });
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.FindingsAndOrders, PageSecurityItemID = SecurityToken.AddFindingsAndOrders)]
        public virtual ActionResult AddFindingsAndOrders(string id)
        {
            int hearingId = id.ToDecrypt().ToInt();

            var viewModel = new FindingsAndOrdersAddViewModel();
            var hearingInfo = UtilityService.ExecStoredProcedureWithResults<pd_HearingGet_spResult>("pd_HearingGet_sp",
                            new pd_HearingGet_spParams() { HearingID = hearingId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).FirstOrDefault();

            if (hearingInfo != null)
            {
                viewModel.CaseID = hearingInfo.CaseID;
                viewModel.HearingID = hearingInfo.HearingID;
                viewModel.HearingType = hearingInfo.HearingType;
                viewModel.HearingDateTime = hearingInfo.HearingDateTime.ToDefaultFormat();
                viewModel.HearingJudge = hearingInfo.HearingJudge;
                viewModel.HearingDept = hearingInfo.HearingDept;

                viewModel.FindingOrderPersonList = UtilityService.ExecStoredProcedureWithResults<pd_HearingFindingOrderPersonGetByHearingFindingOrderID_spResult>("pd_HearingFindingOrderPersonGetByHearingFindingOrderID_sp",
                                    new pd_HearingFindingOrderPersonGetByHearingFindingOrderID_spParams() { HearingID = hearingId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                                    .Select(x => new FindingOrderPerson
                                    {
                                        PersonID = x.PersonID,
                                        PersonDisplayName = x.PersonNameLast + ", " + x.PersonNameFirst,
                                        HearingFindingOrderPersonID = x.HearingFindingOrderPersonID,
                                        RoleID = x.RoleID,
                                        RoleTypeCodeValue = x.RoleTypeCodeValue,
                                        Selected = x.Selected == 1,
                                        IsRoleClient = x.RoleClient == 1
                                    }).ToList();

                viewModel.FindingOrderNoticeList = UtilityService.ExecStoredProcedureWithResults<pd_HearingFindingOrderNoticeGetByHearingFindingOrderID_spResult>("pd_HearingFindingOrderNoticeGetByHearingFindingOrderID_sp",
                                    new pd_HearingFindingOrderNoticeGetByHearingFindingOrderID_spParams() { UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                                    .Select(x => new FindingOrderNotice
                                    {
                                        HearingFindingOrderNoticeID = x.HearingFindingOrderNoticeID,
                                        NoticeID = x.NoticeID,
                                        Notice = x.Notice,
                                        Selected = x.Selected == 1
                                    }).ToList();

                viewModel.FindingOrderTypeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeHearingFindingOrderTypeGetByHearingTypeCodeID_spResult>("pd_CodeHearingFindingOrderTypeGetByHearingTypeCodeID_sp",
                                    new pd_CodeHearingFindingOrderTypeGetByHearingTypeCodeID_spParams() { HearingTypeCodeID = hearingInfo.HearingTypeCodeID.Value, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                                    .Select(x => new SelectListItem
                                    {
                                        Text = x.HearingFindingOrderCodeValue,
                                        Value = x.HearingFindingOrderCodeID.ToString()
                                    }).ToList();
            }


            var noteEntry = UtilityFunctions.NoteGetByEntity(hearingId, 113, 124).FirstOrDefault();
            if (noteEntry != null)
            {
                viewModel.NoteID = noteEntry.NoteID;
                viewModel.NoteEntry = noteEntry.NoteEntry;
                viewModel.NoteSubject = noteEntry.NoteSubject;
                viewModel.NoteEntityCodeID = noteEntry.NoteEntityCodeID;
                viewModel.NoteEntityTypeCodeID = noteEntry.NoteEntityTypeCodeID;

            }

            return View(viewModel);
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.FindingsAndOrders, PageSecurityItemID = SecurityToken.AddFindingsAndOrders)]
        [HttpPost]
        public virtual ActionResult AddFindingsAndOrders(FindingsAndOrdersAddViewModel viewModel)
        {
            #region Manage Note
            if (!viewModel.NoteID.HasValue && !string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                UtilityFunctions.NoteInsert(113, 124, viewModel.HearingID.Value, 0, null, viewModel.NoteEntry, hearingId: viewModel.HearingID);
            }
            else if (viewModel.NoteID.HasValue && !string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                UtilityFunctions.NoteUpdate(viewModel.NoteID ?? 0, viewModel.NoteEntityCodeID.Value, viewModel.NoteEntityTypeCodeID.Value, viewModel.HearingID.Value, 0, viewModel.NoteSubject, viewModel.NoteEntry, hearingId: viewModel.HearingID);
            }
            else if (viewModel.NoteID.HasValue && string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                var pd_NoteDelete_spParams = new pd_NoteDelete_spParams()
                {
                    ID = viewModel.NoteID ?? 0,
                    RecordStateID = 10,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    LoadOption = "Note",
                };
                UtilityService.ExecStoredProcedureWithResults<object>("pd_NoteDelete_sp", pd_NoteDelete_spParams).ToList();
            }
            #endregion

            if (viewModel.FindingsAndOrders != null && viewModel.FindingsAndOrders.Count > 0)
            {

                foreach (var item in viewModel.FindingsAndOrders)
                {
                    var resultId = UtilityService.ExecStoredProcedureScalar("pd_HearingFindingOrderInsert_sp",
                                           new pd_HearingFindingOrderInsert_spParams()
                                           {
                                               HearingID = viewModel.HearingID,
                                               HearingFindingOrderCodeID = item.CodeHearingFindingOrderID,
                                               RecordStateID = 1,
                                               UserID = UserManager.UserExtended.UserID,
                                               BatchLogJobID = Guid.NewGuid(),
                                              HearingFindingOrderComment=item.HearingFindingOrderComment
                                           });

                    var findingOrderId = resultId.ToInt();

                    if (findingOrderId > 0)
                    {
                        foreach (var person in item.FindingOrderPersonList)
                        {
                            UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingFindingOrderPersonInsertByRoleID_sp",
                                           new pd_HearingFindingOrderPersonInsertByRoleID_spParams()
                                           {
                                               RoleID = person.RoleID,
                                               HearingFindingOrderID = findingOrderId,
                                               RecordStateID = 1,
                                               UserID = UserManager.UserExtended.UserID,
                                               BatchLogJobID = Guid.NewGuid()
                                           }).FirstOrDefault();

                        }
                        if (item.FindingOrderNoticeList != null)
                        {
                            foreach (var notice in item.FindingOrderNoticeList)
                            {
                                UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingFindingOrderNoticeInsert_sp",
                                               new pd_HearingFindingOrderNoticeInsert_spParams()
                                               {
                                                   HearingFindingOrderID = findingOrderId,
                                                   HearingFindingOrderNoticeCodeID = notice.NoticeID,
                                                   RecordStateID = 1,
                                                   UserID = UserManager.UserExtended.UserID,
                                                   BatchLogJobID = Guid.NewGuid()
                                               }).FirstOrDefault();

                            }
                        }
                      
                    }
                }
            }
            return Json(new { isSuccess = true });
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.FindingsAndOrders, PageSecurityItemID = SecurityToken.EditFindingsAndOrders)]
        public virtual ActionResult EditFindingsAndOrders(string id, string fId)
        {
            int hearingId = id.ToDecrypt().ToInt();
            int findingsId = fId.ToDecrypt().ToInt();

            var viewModel = new FindingsAndOrdersEditViewModel();
            var hearingInfo = UtilityService.ExecStoredProcedureWithResults<pd_HearingGet_spResult>("pd_HearingGet_sp",
                            new pd_HearingGet_spParams() { HearingID = hearingId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).FirstOrDefault();

            if (hearingInfo != null)
            {
                viewModel.CaseID = hearingInfo.CaseID;
                viewModel.HearingID = hearingInfo.HearingID;
                viewModel.HearingType = hearingInfo.HearingType;
                viewModel.HearingDateTime = hearingInfo.HearingDateTime.ToDefaultFormat();
                viewModel.HearingJudge = hearingInfo.HearingJudge;
                viewModel.HearingDept = hearingInfo.HearingDept;


                var finding = UtilityService.ExecStoredProcedureWithResults<pd_HearingFindingOrderGet_spResult>("pd_HearingFindingOrderGet_sp",
                            new pd_HearingFindingOrderGet_spParams() { HearingFindingOrderID = findingsId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).FirstOrDefault();

                if (finding != null)
                {
                    viewModel.HearingFindingOrderID = finding.HearingFindingOrderID;
                    viewModel.HearingFindingOrderCodeID = finding.HearingFindingOrderCodeID;
                    viewModel.RecordStateID = finding.RecordStateID;
                    viewModel.AgencyID = finding.AgencyID;
                    viewModel.HearingFindingOrderComment = finding.HearingFindingOrderComment;
                }


                viewModel.FindingOrderPersonList = UtilityService.ExecStoredProcedureWithResults<pd_HearingFindingOrderPersonGetByHearingFindingOrderID_spResult>("pd_HearingFindingOrderPersonGetByHearingFindingOrderID_sp",
                            new pd_HearingFindingOrderPersonGetByHearingFindingOrderID_spParams() { HearingFindingOrderID = findingsId, HearingID = hearingId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                            .Select(x => new FindingOrderPerson
                            {
                                PersonID = x.PersonID,
                                PersonDisplayName = x.PersonNameLast + ", " + x.PersonNameFirst,
                                HearingFindingOrderPersonID = x.HearingFindingOrderPersonID,
                                RoleID = x.RoleID,
                                RoleTypeCodeValue = x.RoleTypeCodeValue,
                                Selected = x.Selected == 1,
                                IsRoleClient = x.RoleClient == 1
                            }).ToList();


                viewModel.FindingOrderNoticeList = UtilityService.ExecStoredProcedureWithResults<pd_HearingFindingOrderNoticeGetByHearingFindingOrderID_spResult>("pd_HearingFindingOrderNoticeGetByHearingFindingOrderID_sp",
                                    new pd_HearingFindingOrderNoticeGetByHearingFindingOrderID_spParams() { HearingFindingOrderID = findingsId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                                    .Select(x => new FindingOrderNotice
                                    {
                                        HearingFindingOrderNoticeID = x.HearingFindingOrderNoticeID,
                                        NoticeID = x.NoticeID,
                                        Notice = x.Notice,
                                        Selected = x.Selected == 1
                                    }).ToList();


                viewModel.FindingOrderTypeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeHearingFindingOrderTypeGetByHearingTypeCodeID_spResult>("pd_CodeHearingFindingOrderTypeGetByHearingTypeCodeID_sp",
                                    new pd_CodeHearingFindingOrderTypeGetByHearingTypeCodeID_spParams() { HearingTypeCodeID = findingsId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                                    .Select(x => new SelectListItem
                                    {
                                        Text = x.HearingFindingOrderCodeValue,
                                        Value = x.HearingFindingOrderCodeID.ToString()
                                    }).ToList();
            }


            var noteEntry = UtilityFunctions.NoteGetByEntity(hearingId, 113, 124).FirstOrDefault();
            if (noteEntry != null)
            {
                viewModel.NoteID = noteEntry.NoteID;
                viewModel.NoteEntry = noteEntry.NoteEntry;
                viewModel.NoteSubject = noteEntry.NoteSubject;
                viewModel.NoteEntityCodeID = noteEntry.NoteEntityCodeID;
                viewModel.NoteEntityTypeCodeID = noteEntry.NoteEntityTypeCodeID;

            }

            return View(viewModel);
        }


        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.FindingsAndOrders, PageSecurityItemID = SecurityToken.EditFindingsAndOrders)]
        [HttpPost]
        public virtual ActionResult EditFindingsAndOrders(FindingsAndOrdersEditViewModel viewModel)
        {
            #region Manage Note
            if (!viewModel.NoteID.HasValue && !string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                UtilityFunctions.NoteInsert(113, 124, viewModel.HearingID.Value, 0, null, viewModel.NoteEntry, hearingId: viewModel.HearingID);
            }
            else if (viewModel.NoteID.HasValue && !string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                UtilityFunctions.NoteUpdate(viewModel.NoteID ?? 0, viewModel.NoteEntityCodeID.Value, viewModel.NoteEntityTypeCodeID.Value, viewModel.HearingID.Value, 0, viewModel.NoteSubject, viewModel.NoteEntry, hearingId: viewModel.HearingID);
            }
            else if (viewModel.NoteID.HasValue && string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                var pd_NoteDelete_spParams = new pd_NoteDelete_spParams()
                {
                    ID = viewModel.NoteID ?? 0,
                    RecordStateID = 10,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    LoadOption = "Note",
                };
                UtilityService.ExecStoredProcedureWithResults<object>("pd_NoteDelete_sp", pd_NoteDelete_spParams).ToList();
            }
            #endregion

            if (viewModel.IsFindingUpdate)
            {
                UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingFindingOrderUpdate_sp",
                                                new pd_HearingFindingOrderUpdate_spParams()
                                                {
                                                    HearingFindingOrderID = viewModel.HearingFindingOrderID,
                                                    AgencyID = viewModel.AgencyID,
                                                    HearingID = viewModel.HearingID,
                                                    HearingFindingOrderCodeID = viewModel.HearingFindingOrderCodeID,
                                                    RecordStateID = viewModel.RecordStateID,
                                                    UserID = UserManager.UserExtended.UserID,
                                                    BatchLogJobID = Guid.NewGuid(),
                                                    HearingFindingOrderComment =viewModel.HearingFindingOrderComment
                                                }).FirstOrDefault();
            }


            if (viewModel.FindingOrderPersonList != null && viewModel.FindingOrderPersonList.Count > 0)
            {
                foreach (var person in viewModel.FindingOrderPersonList)
                {
                    if (person.Selected && !person.HearingFindingOrderPersonID.HasValue)
                    {
                        UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingFindingOrderPersonInsertByRoleID_sp",
                                           new pd_HearingFindingOrderPersonInsertByRoleID_spParams()
                                           {
                                               RoleID = person.RoleID,
                                               HearingFindingOrderID = viewModel.HearingFindingOrderID,
                                               RecordStateID = 1,
                                               UserID = UserManager.UserExtended.UserID,
                                               BatchLogJobID = Guid.NewGuid()
                                           }).FirstOrDefault();
                    }
                    else if (!person.Selected && person.HearingFindingOrderPersonID.HasValue)
                    {
                        UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingFindingOrderPersonDelete_sp",
                                                new pd_HearingFindingOrderPersonDelete_spParams()
                                                {
                                                    ID = person.RoleID,
                                                    ID2 = viewModel.HearingFindingOrderID,
                                                    RecordStateID = 10,
                                                    LoadOption= "HearingFindingOrderPerson",
                                                    UserID = UserManager.UserExtended.UserID,
                                                    BatchLogJobID = Guid.NewGuid()
                                                }).FirstOrDefault();
                    }

                }
            }

            if (viewModel.FindingOrderNoticeList != null && viewModel.FindingOrderNoticeList.Count > 0)
            {
                foreach (var notice in viewModel.FindingOrderNoticeList)
                {
                    if (notice.Selected && !notice.HearingFindingOrderNoticeID.HasValue)
                    {
                        UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingFindingOrderNoticeInsert_sp",
                                          new pd_HearingFindingOrderNoticeInsert_spParams()
                                          {
                                              HearingFindingOrderID = viewModel.HearingFindingOrderID,
                                              HearingFindingOrderNoticeCodeID = notice.NoticeID,
                                              RecordStateID = 1,
                                              UserID = UserManager.UserExtended.UserID,
                                              BatchLogJobID = Guid.NewGuid()
                                          }).FirstOrDefault();
                    }
                    else if (!notice.Selected && notice.HearingFindingOrderNoticeID.HasValue)
                    {
                        UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingFindingOrderNoticeDelete_sp",
                                                new pd_HearingFindingOrderNoticeDelete_spParams()
                                                {
                                                    ID = notice.HearingFindingOrderNoticeID.Value,
                                                    RecordStateID = 10,
                                                    LoadOption = "HearingFindingOrderNotice",
                                                    UserID = UserManager.UserExtended.UserID,
                                                    BatchLogJobID = Guid.NewGuid()
                                                }).FirstOrDefault();
                    }

                }
            }

            return Json(new { isSuccess = true });
        }

    }
}