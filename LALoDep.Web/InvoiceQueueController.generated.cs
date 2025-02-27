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
namespace LALoDep.Controllers.Inquiry
{
    public partial class InvoiceQueueController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected InvoiceQueueController(Dummy d) { }

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
        public virtual System.Web.Mvc.ActionResult Download()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Download);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult InvoiceVerifyPopup()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.InvoiceVerifyPopup);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult UpdateInvoice()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateInvoice);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ApproveInvoice()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ApproveInvoice);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public InvoiceQueueController Actions { get { return MVC.InvoiceQueue; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "InvoiceQueue";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "InvoiceQueue";
        [GeneratedCode("T4MVC", "2.0")]
        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Search = "Search";
            public readonly string PrintInvoiceQueue = "PrintInvoiceQueue";
            public readonly string Download = "Download";
            public readonly string InvoiceVerifyPopup = "InvoiceVerifyPopup";
            public readonly string UpdateInvoice = "UpdateInvoice";
            public readonly string ApproveInvoice = "ApproveInvoice";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Search = "Search";
            public const string PrintInvoiceQueue = "PrintInvoiceQueue";
            public const string Download = "Download";
            public const string InvoiceVerifyPopup = "InvoiceVerifyPopup";
            public const string UpdateInvoice = "UpdateInvoice";
            public const string ApproveInvoice = "ApproveInvoice";
        }


        static readonly ActionParamsClass_Search s_params_Search = new ActionParamsClass_Search();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Search SearchParams { get { return s_params_Search; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Search
        {
            public readonly string viewModel = "viewModel";
        }
        static readonly ActionParamsClass_Download s_params_Download = new ActionParamsClass_Download();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Download DownloadParams { get { return s_params_Download; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Download
        {
            public readonly string file = "file";
        }
        static readonly ActionParamsClass_InvoiceVerifyPopup s_params_InvoiceVerifyPopup = new ActionParamsClass_InvoiceVerifyPopup();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_InvoiceVerifyPopup InvoiceVerifyPopupParams { get { return s_params_InvoiceVerifyPopup; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_InvoiceVerifyPopup
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_UpdateInvoice s_params_UpdateInvoice = new ActionParamsClass_UpdateInvoice();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UpdateInvoice UpdateInvoiceParams { get { return s_params_UpdateInvoice; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UpdateInvoice
        {
            public readonly string id = "id";
            public readonly string invoiceDetails = "invoiceDetails";
        }
        static readonly ActionParamsClass_ApproveInvoice s_params_ApproveInvoice = new ActionParamsClass_ApproveInvoice();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ApproveInvoice ApproveInvoiceParams { get { return s_params_ApproveInvoice; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ApproveInvoice
        {
            public readonly string id = "id";
            public readonly string invoiceDetails = "invoiceDetails";
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
                public readonly string InvoiceVerifyPopup = "InvoiceVerifyPopup";
                public readonly string Search = "Search";
            }
            public readonly string InvoiceVerifyPopup = "~/Views/InvoiceQueue/InvoiceVerifyPopup.cshtml";
            public readonly string Search = "~/Views/InvoiceQueue/Search.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_InvoiceQueueController : LALoDep.Controllers.Inquiry.InvoiceQueueController
    {
        public T4MVC_InvoiceQueueController() : base(Dummy.Instance) { }

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
        partial void SearchOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, LALoDep.Models.Inquiry.InvoiceQueueViewModel viewModel);

        [NonAction]
        public override System.Web.Mvc.JsonResult Search(LALoDep.Models.Inquiry.InvoiceQueueViewModel viewModel)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.Search);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "viewModel", viewModel);
            SearchOverride(callInfo, viewModel);
            return callInfo;
        }

        [NonAction]
        partial void PrintInvoiceQueueOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult PrintInvoiceQueue()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.PrintInvoiceQueue);
            PrintInvoiceQueueOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void DownloadOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string file);

        [NonAction]
        public override System.Web.Mvc.ActionResult Download(string file)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Download);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "file", file);
            DownloadOverride(callInfo, file);
            return callInfo;
        }

        [NonAction]
        partial void InvoiceVerifyPopupOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string id);

        [NonAction]
        public override System.Web.Mvc.ActionResult InvoiceVerifyPopup(string id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.InvoiceVerifyPopup);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            InvoiceVerifyPopupOverride(callInfo, id);
            return callInfo;
        }

        [NonAction]
        partial void UpdateInvoiceOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int id, System.Collections.Generic.List<LALoDep.Models.Inquiry.InvoiceDetails> invoiceDetails);

        [NonAction]
        public override System.Web.Mvc.ActionResult UpdateInvoice(int id, System.Collections.Generic.List<LALoDep.Models.Inquiry.InvoiceDetails> invoiceDetails)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateInvoice);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "invoiceDetails", invoiceDetails);
            UpdateInvoiceOverride(callInfo, id, invoiceDetails);
            return callInfo;
        }

        [NonAction]
        partial void ApproveInvoiceOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int id, System.Collections.Generic.List<LALoDep.Models.Inquiry.InvoiceDetails> invoiceDetails);

        [NonAction]
        public override System.Web.Mvc.ActionResult ApproveInvoice(int id, System.Collections.Generic.List<LALoDep.Models.Inquiry.InvoiceDetails> invoiceDetails)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ApproveInvoice);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "invoiceDetails", invoiceDetails);
            ApproveInvoiceOverride(callInfo, id, invoiceDetails);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
