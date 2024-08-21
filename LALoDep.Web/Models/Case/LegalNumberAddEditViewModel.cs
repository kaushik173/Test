using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LALoDep.Domain;
using LALoDep.Domain.pd_Case;
using System.Web.Mvc;
using LALoDep.Domain.pd_LegalNumber;
using LALoDep.Domain.NgInvoice;

namespace LALoDep.Models.Case
{
    public class LegalNumberAddEditViewModel
    {
        public IEnumerable<SelectListItem> LegalNumberTypeList { get; set; }
        public int? LegalNumberTypeCodeID { get; set; }
        public int LegalNumberID { get; set; }
        public string LegalNumberEntry { get; set; }
        public string LegalNumberComment { get; set; }
        public int RecordStateID { get; set; }
        public int PersonID { get; set; }
        public int AgencyID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<pd_LegalNumberGetByPersonID_spResult> LegalNumbers { get; set; }
        public LegalNumberAddEditViewModel()
        {
            LegalNumbers = new List<pd_LegalNumberGetByPersonID_spResult>();
        }
    }

    public class InvoicesViewModel
    {

        public string FilterByEnum { get; set; }

        public IEnumerable<SelectListItem> FilterByList
        {
            get; set;
        }
        public List<NgInvoice_GetForCase_spResult> InvoiceList { get; set; }
    }
    public class InvoiceAddEditViewModel : NgInvoice_Get_spResult
    {
      

        public IEnumerable<SelectListItem> InvoiceStatusList
        {
            get; set;
        }
        public IEnumerable<SelectListItem> ChildTypeList
        {
            get; set;
        }
        public IEnumerable<SelectListItem> StateList  
        {
            get; set;
        }
        public IEnumerable<SelectListItem> CACountyList  
    {
            get; set;
        }
        public IEnumerable<SelectListItem> ExpenseStatusList 
        {
            get; set;
        }

        public IEnumerable<SelectListItem> PartyTypeList 
        {
            get; set;
        }
        public IEnumerable<SelectListItem> CounselList 
        {
            get; set;
        }


        public IEnumerable<NgInvoice_GetMinors_spResult> ChildNMDCurrentPlacmentList
        {
            get; set;
        }
        public IEnumerable<NgInvoice_GetTotals_spResult> InvoiceGetTotals
        {
            get; set;
        }
        public List<NgInvoice_GetRecordTime_spResult> RecordTimeDetailsList { get; set; }
        public List<NgInvoice_GetExpenses_spResult> ExpenseDetailsList { get; set; }

        public List<NgInvoice_GetCounselHistory_spResult> CounselHistoryList { get; set; }



        public List<NgInvoiceMinorIUD_spParams> ChildSaveParamList { get; set; }

        public List<NgInvoiceDetail_Expense_IUD_spParams> ExpenseSaveParamList { get; set; }

        public List<NgInvoiceDetail_RecordTime_IUD_spParams> RecordTimeSaveParamList { get; set; }
        public List<NgInvoiceCounselIUD_spParams> NgInvoiceCounselIUDList { get; set; }

        public string ButtonID { get; set; }
        public string ReturnPageUrl { get; set; }
        public string MyInvoiceQueuePersonName { get; set; }

        public InvoiceAddEditViewModel()
        { 
             
             

     
            InvoiceStatusList = new List<SelectListItem>();
            CounselList = new List<SelectListItem>();
            PartyTypeList = new List<SelectListItem>();
            CACountyList = new List<SelectListItem>();
            StateList = new List<SelectListItem>();
            ChildTypeList = new List<SelectListItem>();
            ChildNMDCurrentPlacmentList = new List<NgInvoice_GetMinors_spResult>();
            InvoiceGetTotals = new List<NgInvoice_GetTotals_spResult>();
            RecordTimeDetailsList = new List<NgInvoice_GetRecordTime_spResult>();
            ExpenseDetailsList = new List<NgInvoice_GetExpenses_spResult>();

            CounselHistoryList = new List<NgInvoice_GetCounselHistory_spResult>();

            ChildSaveParamList = new List<NgInvoiceMinorIUD_spParams>();

            ExpenseSaveParamList = new List<NgInvoiceDetail_Expense_IUD_spParams>();

            RecordTimeSaveParamList = new List<NgInvoiceDetail_RecordTime_IUD_spParams>();
            NgInvoiceCounselIUDList = new List<NgInvoiceCounselIUD_spParams>();
        }
    }
}