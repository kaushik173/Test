using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LALoDep.Core.Custom.Utility;
using LALoDep.Domain;

namespace LALoDep.Custom
{
    /// <summary>
    /// Created By: 
    /// Created On: 
    /// Purpose: Class for Session variables
    /// Last Updated On: 22 Jan, 2014
    /// Last Updated By: Kays Dev 
    /// Reason: New session variable for Case Number's AgencyID and BranchID
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public class UserExtended
    {
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public int StaffID { get; set; }
        public int AgencyID { get; set; }
        public int BranchID { get; set; }
        public string FullName { get; set; }
        public Guid Guid { get; set; }

        public string ThemeUrl { get; set; }
        public string ZoomCssClass { get; set; }

        // Case Details
        public int CaseID { get; set; }
        public string Client { get; set; }
        public string PDAPDNumber { get; set; }
        public string ApptDate { get; set; }
        public string Status { get; set; }
        public string CourtNumber { get; set; }
        public string DaCaNumber { get; set; }
        public string Class { get; set; }
        public string Type { get; set; }
        public string Attorney { get; set; }
        public int? AttorneyRoleID { get; set; }
        public int? JcatsGroupID { get; set; }

        //Case Number's AgencyID
        public int CaseNumberAgencyID { get; set; }
        //Case Number's BranchID
        public int CaseNumberBranchID { get; set; }
        public string NextHearingDate { get; set; }

        public string CaseJcatsNumber { get; set; }

        //Case Sealed or Secure
        public string AccessDeniedReason { get; set; }
        public int CaseStateID { get; set; }
        //Preserve Important Case Details
        public int BeforeSecureCaseID { get; set; }
        public string BeforeSecurePDAPDNumber { get; set; }

        public int SystemAdminFlag { get; set; }
        public string AdminLoginName { get; set; }

        public string EncryptCaseID { get { return Utility.Encrypt(CaseID.ToString()); } }


        public int WritsAppealsFlag { get; set; }

        public string TrialCourtNumber { get; set; }
        public string TrialAtty { get; set; }
        public string AppDiv { get; set; }
        public string DCANumber { get; set; }
        public string SupCTNumber { get; set; }

        public int AttorneyPersonID { get; set; }

        public int InitialLogin { get; set; }
        public string DefaultLandingPage { get; set; }
        public int SessionTimeOut { get; set; }

        public string PrintDocumentOn { get; set; }

        public string PageLayout { get; set; }

        public bool HyperlinkUnderline { get; set; }
    }


    public class CaseSummaryBarInfo
    {
        private static CaseSummaryData _caseInfo { get; set; }


        public static void UpdateCaseInfo(CaseSummaryData caseSummaryData)
        {
            var sessionId = GetCurrentWindowID();
            if (sessionId != "")
            {
                HttpContext.Current.Session[sessionId] = caseSummaryData;
                NGJcatsUserSession.SaveSessionData(HttpContext.Current.Session.SessionID, caseSummaryData.CaseID, UserEnvironment.UserManager.UserExtended.UserID, sessionId, "AOC");
            }

        }
        public static CaseSummaryData CaseInfo()
        {

            var sessionId = GetCurrentWindowID();
            if (sessionId != "")
            {
                try
                {
                    if (HttpContext.Current.Session[sessionId] != null)
                    {
                        _caseInfo = (CaseSummaryData)HttpContext.Current.Session[sessionId];

                    }
                    else
                    {
                        _caseInfo = new CaseSummaryData();
                        HttpContext.Current.Session[sessionId] = _caseInfo;
                    }
                }
                catch
                {
                    _caseInfo = new CaseSummaryData();
                    HttpContext.Current.Session[sessionId] = _caseInfo;
                }
            }
            return _caseInfo;
        }
        public static string GetCurrentWindowID()
        {
            //if (HttpContext.Current.Request.Cookies["CurrentWindowID"] != null)
            //{
            //    return HttpContext.Current.Request.Cookies["CurrentWindowID"].Value;
            //}
            if (HttpContext.Current.Request.QueryString["CurrentSessionGuid"] != null && HttpContext.Current.Request.QueryString["_uniquerequest"] != null)
            {
               
                return HttpContext.Current.Request.QueryString["CurrentSessionGuid"];
            }
            else if (HttpContext.Current.Request.RequestContext.RouteData.Values["guid"] != null && HttpContext.Current.Request.RequestContext.RouteData.Values["guid"].ToString() != "")
            {
               
                return HttpContext.Current.Request.RequestContext.RouteData.Values["guid"].ToString();
            }
             

            return "";
        }
    }
    [Serializable]  //Added by Humair. Serializable for SQL Server Session State.............
    public class CaseSummaryData
    {
        #region Properties

        // Case Details
        public int CaseID { get; set; }
        public string Client { get; set; }
        public string PDAPDNumber { get; set; }
        public string ApptDate { get; set; }
        public string Status { get; set; }

        public string Attorney { get; set; }

        //Case Number's AgencyID
        public int CaseNumberAgencyID { get; set; }
        //Case Number's BranchID
        public string EncryptCaseID
        {
            get { return Utility.Encrypt(CaseID.ToString()); }
        }



        public string CaseJcatsNumber { get; set; }

        public string NextHearingDate { get; set; }

        #endregion

    }



    public class NGJcatsUserSession : System.Web.IPartitionResolver
    {
        private static string ConnectionString
        {
            get
            {

                return LALoDepEntities.GetSessionConnectionString();
            }
        }
        public void Initialize()
        {
            //Empty Initializer
        }

        public string ResolvePartition(object key)
        {
            return ConnectionString;
        }

        public static void SaveSessionData(string sessionId, int caseId, int userId, string javascriptTabId, string appName)
        {
            var db = new DbManager(ConnectionString);

            db.AddInParam("SessionID", sessionId);
            db.AddInParam("CaseID", caseId);
            db.AddInParam("UserID", userId);
            db.AddInParam("JavascriptTabID", javascriptTabId);
            db.AddInParam("AppName", appName);

            db.ExecuteNonQuery("InsertUserSessionData");
        }
    }
}