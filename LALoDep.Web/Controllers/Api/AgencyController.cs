
//using LALoDep.Domain.pd_JcatsGroup;
//using LALoDep.Domain.Services;
//using LALoDep.Custom;
//using LALoDep.Models.Api;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Http;

//namespace LALoDep.Controllers.Api
//{
//    public class AgencyController : ApiBaseController
//    {
//        public AgencyController(UserManager _userManager, IUtilityService _utilityService) : base(_userManager, _utilityService) { }

//        [HttpGet]
//        public ApiResult<List<pd_JcatsGroupAgencyGetByJcatsUserID_spResults>> AgencyGetByJcatsUserID()
//        {
//            var result = new ApiResult<List<pd_JcatsGroupAgencyGetByJcatsUserID_spResults>> { IsSuccess = true };
//            try
//            {
//                result.Result = UtilityService.ExecStoredProcedureWithResults<pd_JcatsGroupAgencyGetByJcatsUserID_spResults>("pd_JcatsGroupAgencyGetByJcatsUserID_sp", new pd_JcatsGroupAgencyGetByJcatsUserID_spParams()
//                {
//                    UserID = UserManager.UserExtended.UserID,
//                    BatchLogJobID = Guid.NewGuid()
//                }).ToList();
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
