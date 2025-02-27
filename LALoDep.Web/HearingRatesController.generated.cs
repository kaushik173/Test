// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo. Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
// 0114: suppress "Foo.BarController.Baz()' hides inherited member 'Qux.BarController.Baz()'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword." when an action (with an argument) overrides an action in a parent controller
#pragma warning disable 1591, 3008, 3009, 0108, 0114
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace LALoDep.Controllers.Administration
{
    public partial class HearingRatesController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected HearingRatesController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult AddEdit()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AddEdit);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult AddEditHearingRates()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.AddEditHearingRates);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult SaveDeleteHearingRates()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.SaveDeleteHearingRates);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public HearingRatesController Actions { get { return MVC.HearingRates; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "HearingRates";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "HearingRates";
        [GeneratedCode("T4MVC", "2.0")]
        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Search = "Search";
            public readonly string SearchHearingData = "SearchHearingData";
            public readonly string AddEdit = "AddEdit";
            public readonly string AddEditHearingRates = "AddEditHearingRates";
            public readonly string SaveDeleteHearingRates = "SaveDeleteHearingRates";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Search = "Search";
            public const string SearchHearingData = "SearchHearingData";
            public const string AddEdit = "AddEdit";
            public const string AddEditHearingRates = "AddEditHearingRates";
            public const string SaveDeleteHearingRates = "SaveDeleteHearingRates";
        }


        static readonly ActionParamsClass_AddEdit s_params_AddEdit = new ActionParamsClass_AddEdit();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_AddEdit AddEditParams { get { return s_params_AddEdit; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_AddEdit
        {
            public readonly string AgencyID = "AgencyID";
            public readonly string HearingTypeID = "HearingTypeID";
        }
        static readonly ActionParamsClass_AddEditHearingRates s_params_AddEditHearingRates = new ActionParamsClass_AddEditHearingRates();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_AddEditHearingRates AddEditHearingRatesParams { get { return s_params_AddEditHearingRates; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_AddEditHearingRates
        {
            public readonly string AgencyID = "AgencyID";
            public readonly string HearingTypeID = "HearingTypeID";
        }
        static readonly ActionParamsClass_SaveDeleteHearingRates s_params_SaveDeleteHearingRates = new ActionParamsClass_SaveDeleteHearingRates();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SaveDeleteHearingRates SaveDeleteHearingRatesParams { get { return s_params_SaveDeleteHearingRates; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SaveDeleteHearingRates
        {
            public readonly string viewModel = "viewModel";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string AddEdit = "AddEdit";
                public readonly string Search = "Search";
            }
            public readonly string AddEdit = "~/Views/HearingRates/AddEdit.cshtml";
            public readonly string Search = "~/Views/HearingRates/Search.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_HearingRatesController : LALoDep.Controllers.Administration.HearingRatesController
    {
        public T4MVC_HearingRatesController() : base(Dummy.Instance) { }

        [NonAction]
        partial void SearchOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Search()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Search);
            SearchOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void SearchHearingDataOverride(T4MVC_System_Web_Mvc_JsonResult callInfo);

        [NonAction]
        public override System.Web.Mvc.JsonResult SearchHearingData()
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.SearchHearingData);
            SearchHearingDataOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void AddEditOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string AgencyID, string HearingTypeID);

        [NonAction]
        public override System.Web.Mvc.ActionResult AddEdit(string AgencyID, string HearingTypeID)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AddEdit);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "AgencyID", AgencyID);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "HearingTypeID", HearingTypeID);
            AddEditOverride(callInfo, AgencyID, HearingTypeID);
            return callInfo;
        }

        [NonAction]
        partial void AddEditHearingRatesOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, string AgencyID, string HearingTypeID);

        [NonAction]
        public override System.Web.Mvc.JsonResult AddEditHearingRates(string AgencyID, string HearingTypeID)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.AddEditHearingRates);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "AgencyID", AgencyID);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "HearingTypeID", HearingTypeID);
            AddEditHearingRatesOverride(callInfo, AgencyID, HearingTypeID);
            return callInfo;
        }

        [NonAction]
        partial void SaveDeleteHearingRatesOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, LALoDep.Models.Administration.HearingRatesAddEditViewModel viewModel);

        [NonAction]
        public override System.Web.Mvc.JsonResult SaveDeleteHearingRates(LALoDep.Models.Administration.HearingRatesAddEditViewModel viewModel)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.SaveDeleteHearingRates);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "viewModel", viewModel);
            SaveDeleteHearingRatesOverride(callInfo, viewModel);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
