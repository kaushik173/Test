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
    public partial class CountyCounselListController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected CountyCounselListController(Dummy d) { }

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
        public virtual System.Web.Mvc.JsonResult CountyCounselDelete()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.CountyCounselDelete);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult AddEditCountyCounsel()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AddEditCountyCounsel);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult GetAllCountyCheckboxes()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetAllCountyCheckboxes);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult UpdateCountyCounsel()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.UpdateCountyCounsel);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public CountyCounselListController Actions { get { return MVC.CountyCounselList; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "CountyCounselList";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "CountyCounselList";
        [GeneratedCode("T4MVC", "2.0")]
        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Search = "Search";
            public readonly string CountyCounselDelete = "CountyCounselDelete";
            public readonly string AddEditCountyCounsel = "AddEditCountyCounsel";
            public readonly string GetAllCountyCheckboxes = "GetAllCountyCheckboxes";
            public readonly string UpdateCountyCounsel = "UpdateCountyCounsel";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Search = "Search";
            public const string CountyCounselDelete = "CountyCounselDelete";
            public const string AddEditCountyCounsel = "AddEditCountyCounsel";
            public const string GetAllCountyCheckboxes = "GetAllCountyCheckboxes";
            public const string UpdateCountyCounsel = "UpdateCountyCounsel";
        }


        static readonly ActionParamsClass_Search s_params_Search = new ActionParamsClass_Search();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Search SearchParams { get { return s_params_Search; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Search
        {
            public readonly string parameters = "parameters";
        }
        static readonly ActionParamsClass_CountyCounselDelete s_params_CountyCounselDelete = new ActionParamsClass_CountyCounselDelete();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_CountyCounselDelete CountyCounselDeleteParams { get { return s_params_CountyCounselDelete; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_CountyCounselDelete
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_AddEditCountyCounsel s_params_AddEditCountyCounsel = new ActionParamsClass_AddEditCountyCounsel();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_AddEditCountyCounsel AddEditCountyCounselParams { get { return s_params_AddEditCountyCounsel; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_AddEditCountyCounsel
        {
            public readonly string Id = "Id";
        }
        static readonly ActionParamsClass_GetAllCountyCheckboxes s_params_GetAllCountyCheckboxes = new ActionParamsClass_GetAllCountyCheckboxes();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GetAllCountyCheckboxes GetAllCountyCheckboxesParams { get { return s_params_GetAllCountyCheckboxes; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GetAllCountyCheckboxes
        {
            public readonly string Id = "Id";
        }
        static readonly ActionParamsClass_UpdateCountyCounsel s_params_UpdateCountyCounsel = new ActionParamsClass_UpdateCountyCounsel();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UpdateCountyCounsel UpdateCountyCounselParams { get { return s_params_UpdateCountyCounsel; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UpdateCountyCounsel
        {
            public readonly string person = "person";
            public readonly string checkboxes = "checkboxes";
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
                public readonly string AddEditCountyCounsel = "AddEditCountyCounsel";
                public readonly string Search = "Search";
            }
            public readonly string AddEditCountyCounsel = "~/Views/CountyCounselList/AddEditCountyCounsel.cshtml";
            public readonly string Search = "~/Views/CountyCounselList/Search.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_CountyCounselListController : LALoDep.Controllers.Administration.CountyCounselListController
    {
        public T4MVC_CountyCounselListController() : base(Dummy.Instance) { }

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
        partial void SearchOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, LALoDep.Models.Administration.CountyCounselListViewModel parameters);

        [NonAction]
        public override System.Web.Mvc.JsonResult Search(LALoDep.Models.Administration.CountyCounselListViewModel parameters)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.Search);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "parameters", parameters);
            SearchOverride(callInfo, parameters);
            return callInfo;
        }

        [NonAction]
        partial void CountyCounselDeleteOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, string id);

        [NonAction]
        public override System.Web.Mvc.JsonResult CountyCounselDelete(string id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.CountyCounselDelete);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            CountyCounselDeleteOverride(callInfo, id);
            return callInfo;
        }

        [NonAction]
        partial void AddEditCountyCounselOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string Id);

        [NonAction]
        public override System.Web.Mvc.ActionResult AddEditCountyCounsel(string Id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AddEditCountyCounsel);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "Id", Id);
            AddEditCountyCounselOverride(callInfo, Id);
            return callInfo;
        }

        [NonAction]
        partial void GetAllCountyCheckboxesOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, string Id);

        [NonAction]
        public override System.Web.Mvc.JsonResult GetAllCountyCheckboxes(string Id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetAllCountyCheckboxes);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "Id", Id);
            GetAllCountyCheckboxesOverride(callInfo, Id);
            return callInfo;
        }

        [NonAction]
        partial void UpdateCountyCounselOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, LALoDep.Models.Administration.CountyCounselPerson person, LALoDep.Models.Administration.CountyCounselAgencyCheckbox[] checkboxes);

        [NonAction]
        public override System.Web.Mvc.JsonResult UpdateCountyCounsel(LALoDep.Models.Administration.CountyCounselPerson person, LALoDep.Models.Administration.CountyCounselAgencyCheckbox[] checkboxes)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.UpdateCountyCounsel);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "person", person);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "checkboxes", checkboxes);
            UpdateCountyCounselOverride(callInfo, person, checkboxes);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
