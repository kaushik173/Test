using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.CaseAttribute;
using LALoDep.Domain.pd_Allegation;
using LALoDep.Domain.pd_Association;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain.pd_Note;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.Services;
using LALoDep.Domain.sup_Case;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Task;
using LALoDep.Domain.Agency;
using LALoDep.Core.Custom.Extensions;
using System.Collections.Generic;
using LALoDep.Custom;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_Address;
using System.Globalization;
using pd_CodeGetNewNoteTypeByCaseID_spParams = LALoDep.Domain.pd_Code.pd_CodeGetNewNoteTypeByCaseID_spParams;
using pd_CodeGetNewNoteTypeByCaseID_spResult = LALoDep.Domain.pd_Code.pd_CodeGetNewNoteTypeByCaseID_spResult;
using LALoDep.Domain.pd_Users.Edit;

namespace LALoDep.Controllers
{
    [AuthenticationAuthorize]
    public partial class TaskController
    {

         [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CalendarNumbering, PageSecurityItemID = SecurityToken.CalendarNumbering)]
        public virtual ActionResult CalendarNumbering()
        {
            var model = new CalendarNumberingViewModel();
            model.AgencyList = UtilityService.ExecStoredProcedureWithResults<pd_AgencyGetHomeByPersonID_spResult>(
                "pd_AgencyGetHomeByPersonID_sp", new pd_AgencyGetHomeByPersonID_spParams
                {
                    PersonID = UserManager.UserExtended.PersonID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = new Guid(),
                }).Select(o => new SelectListItem() { Value = o.AgencyID.ToString(), Text = o.AgencyName }).ToList();

            if (model.AgencyList.Count == 1)
                model.AgencyID = model.AgencyList[0].Value.ToInt();

            model.DepartmentList = LALoDep.Custom.UtilityFunctions.CodeGetByTypeIdAndUserId(30, sortOption: "CodeValue").ToList();


            return View(model);
        }

        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CalendarNumbering, PageSecurityItemID = SecurityToken.CalendarNumbering)]

        public virtual JsonResult CalendarNumbering(CalendarNumberingViewModel model)
        {

            var result = UtilityService.ExecStoredProcedureWithResults<qcal_CalendarNumbering_spResult>(
                        "qcal_CalendarNumbering_sp",
                        new qcal_CalendarNumbering_spParams()
                        {
                            UserID = UserManager.UserExtended.UserID,

                            BatchLogJobID = Guid.NewGuid(),
                            AgencyID = model.AgencyID,
                            DepartmentCodeID = model.DepartmentID,
                            HearingDate = model.Date.ToDateTime(),SortOption=model.SortOption
                        }).ToList();

            return Json(new DataTablesResponse(0, result, result.Count, result.Count));
        }
        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CalendarNumbering, PageSecurityItemID = SecurityToken.CalendarNumbering)]

        public virtual JsonResult CalendarNumberingUpdate(CalendarNumberingViewModel model)
        {
            if (model.CalNbrs.Count > 0)
            {
                foreach (var item in model.CalNbrs)
                {

                    UtilityService.ExecStoredProcedureWithoutResultADO(
                        "qcal_CalendarNumberingUpdate_sp",
                        new qcal_CalendarNumberingUpdate_spParams()
                        {
                            UserID = UserManager.UserExtended.UserID,

                            BatchLogJobID =   Guid.NewGuid(),
                            HearingID = item.HearingID,
                            CalNbr = item.CalNbr

                        });
                }
            }

            return Json(new { Status = "Done", Model = model });
        }



    }

}