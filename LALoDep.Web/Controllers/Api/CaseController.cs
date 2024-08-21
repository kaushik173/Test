//using LALoDep.Domain.pd_Case;
//using LALoDep.Domain.Services;
//using LALoDep.Core.Custom.Extensions;
//using LALoDep.Custom;
//using LALoDep.Models.Api;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Http;

//namespace LALoDep.Controllers.Api
//{
//    public partial class CaseController : ApiBaseController
//    {
//        public CaseController(UserManager _userManager, IUtilityService _utilityService) : base(_userManager, _utilityService) { }

//        [HttpPost]
//        public ApiResult<List<pd_CaseSearch_spResults>> Search(CaseSearchModel searchParams)
//        {
//            var result = new ApiResult<List<pd_CaseSearch_spResults>> { IsSuccess = true };
//            try
//            {
//                Guid? wtCaseSearchGUID = null;
//                if (!searchParams.SearchGuid.IsNullOrEmpty() && searchParams.SearchGuid.Length > 0)
//                    wtCaseSearchGUID = Guid.Parse(searchParams.SearchGuid);

//                result.Result = UtilityService.ExecStoredProcedureWithResults<pd_CaseSearch_spResults>("pd_CaseSearch_sp", new pd_CaseSearch_spParams
//                {
//                    LastName = searchParams.LastName?.Trim(),
//                    FirstName = searchParams.FirstName?.Trim(),
//                    DocketNumber = searchParams.DocketNumber?.Trim(),
//                    CaseNumber = searchParams.JcatsNumber?.Trim(),
//                    HHSA = searchParams.HHSA?.Trim(),
//                    AgencyID = searchParams.AgencyID,
//                    Appointment = 1,
//                    SortID = 1,

//                    GUID = wtCaseSearchGUID,
//                    StartRecord = searchParams.StartRecord,
//                    Range = searchParams.Range,

//                    UserID = UserManager.UserExtended.UserID,
//                    BatchLogJobID = Guid.NewGuid(),

//                }).ToList();

//            }
//            catch (Exception ex)
//            {
//                result.Message = ex.Message;
//                result.IsSuccess = false;
//            }
//            return result;
//        }

//        [HttpGet]
//        public ApiResult<LALoDep.Domain.pd_CaseGet_sp_Result> GetSummary(int caseId)
//        {
//            var result = new ApiResult<LALoDep.Domain.pd_CaseGet_sp_Result> { IsSuccess = true };
//            try
//            {
//                var summary = UtilityService.Context.pd_CaseGet_sp(caseId, UserManager.UserExtended.UserID, Guid.NewGuid()).FirstOrDefault();
//                if (summary != null)
//                {
//                    result.Result = summary;
//                }
//                else
//                {
//                    result.IsSuccess = false;
//                    result.Message = "No case found";
//                }
//            }
//            catch (Exception ex)
//            {
//                result.Message = ex.Message;
//                result.IsSuccess = false;
//            }
//            return result;
//        }
//    }
//}
