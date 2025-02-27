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
namespace LALoDep.Controllers.Case
{
    public partial class MotionsController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected MotionsController(Dummy d) { }

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
        public virtual System.Web.Mvc.JsonResult GetMotions()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetMotions);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult MotionAddEdit()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.MotionAddEdit);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult MotionAddEditSave()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.MotionAddEditSave);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult MotionDelete()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.MotionDelete);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public MotionsController Actions { get { return MVC.Motions; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Motions";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Motions";
        [GeneratedCode("T4MVC", "2.0")]
        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string List = "List";
            public readonly string GetPetitions = "GetPetitions";
            public readonly string GetMotions = "GetMotions";
            public readonly string MotionAddEdit = "MotionAddEdit";
            public readonly string MotionAddEditSave = "MotionAddEditSave";
            public readonly string MotionDelete = "MotionDelete";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string List = "List";
            public const string GetPetitions = "GetPetitions";
            public const string GetMotions = "GetMotions";
            public const string MotionAddEdit = "MotionAddEdit";
            public const string MotionAddEditSave = "MotionAddEditSave";
            public const string MotionDelete = "MotionDelete";
        }


        static readonly ActionParamsClass_GetMotions s_params_GetMotions = new ActionParamsClass_GetMotions();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GetMotions GetMotionsParams { get { return s_params_GetMotions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GetMotions
        {
            public readonly string petitionID = "petitionID";
        }
        static readonly ActionParamsClass_MotionAddEdit s_params_MotionAddEdit = new ActionParamsClass_MotionAddEdit();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_MotionAddEdit MotionAddEditParams { get { return s_params_MotionAddEdit; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_MotionAddEdit
        {
            public readonly string id = "id";
            public readonly string petitionID = "petitionID";
        }
        static readonly ActionParamsClass_MotionAddEditSave s_params_MotionAddEditSave = new ActionParamsClass_MotionAddEditSave();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_MotionAddEditSave MotionAddEditSaveParams { get { return s_params_MotionAddEditSave; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_MotionAddEditSave
        {
            public readonly string viewModel = "viewModel";
        }
        static readonly ActionParamsClass_MotionDelete s_params_MotionDelete = new ActionParamsClass_MotionDelete();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_MotionDelete MotionDeleteParams { get { return s_params_MotionDelete; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_MotionDelete
        {
            public readonly string id = "id";
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
                public readonly string List = "List";
                public readonly string MotionAddEdit = "MotionAddEdit";
            }
            public readonly string List = "~/Views/Motions/List.cshtml";
            public readonly string MotionAddEdit = "~/Views/Motions/MotionAddEdit.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_MotionsController : LALoDep.Controllers.Case.MotionsController
    {
        public T4MVC_MotionsController() : base(Dummy.Instance) { }

        [NonAction]
        partial void ListOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult List()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.List);
            ListOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void GetPetitionsOverride(T4MVC_System_Web_Mvc_JsonResult callInfo);

        [NonAction]
        public override System.Web.Mvc.JsonResult GetPetitions()
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetPetitions);
            GetPetitionsOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void GetMotionsOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, string petitionID);

        [NonAction]
        public override System.Web.Mvc.JsonResult GetMotions(string petitionID)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetMotions);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "petitionID", petitionID);
            GetMotionsOverride(callInfo, petitionID);
            return callInfo;
        }

        [NonAction]
        partial void MotionAddEditOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string id, string petitionID);

        [NonAction]
        public override System.Web.Mvc.ActionResult MotionAddEdit(string id, string petitionID)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.MotionAddEdit);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "petitionID", petitionID);
            MotionAddEditOverride(callInfo, id, petitionID);
            return callInfo;
        }

        [NonAction]
        partial void MotionAddEditSaveOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, LALoDep.Models.Case.MotionAddEditViewModel viewModel);

        [NonAction]
        public override System.Web.Mvc.JsonResult MotionAddEditSave(LALoDep.Models.Case.MotionAddEditViewModel viewModel)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.MotionAddEditSave);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "viewModel", viewModel);
            MotionAddEditSaveOverride(callInfo, viewModel);
            return callInfo;
        }

        [NonAction]
        partial void MotionDeleteOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, string id);

        [NonAction]
        public override System.Web.Mvc.JsonResult MotionDelete(string id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.MotionDelete);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            MotionDeleteOverride(callInfo, id);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
