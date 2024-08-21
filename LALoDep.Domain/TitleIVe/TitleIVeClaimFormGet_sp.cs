namespace LALoDep.Domain.TitleIVe
{
    public class TitleIVeClaimFormGet_spParams
    {
        public int? ErrorID { get; set; }
        public int? InvoiceID { get; set; }
        public int? UserID { get; set; }

    }


    public class TitleIVeClaimFormGet_spResult
    {
        
        public string ContractorName { get; set; }
        public string StreetAddress { get; set; }
        public string CityStateZip { get; set; }
        public string Phone { get; set; }
        public decimal? PercentageDependency { get; set; }
        public int? NbrChildrenCasesWorked { get; set; }
        public decimal? PersonnelCostsCACFundAmt { get; set; }
        public decimal? PersonelCostsFFDRPFundAmt { get; set; }
        public decimal? PersonnelCostsEligibleCost { get; set; }
        public decimal? ProfessionalServicesCACFundAmt { get; set; }
        public decimal? ProfessionalServicesFFDRPFundAmt { get; set; }
        public decimal? ProfessionalServicesEligibleCost { get; set; }
        public decimal? OperatingExpensesCACFundAmt { get; set; }
        public decimal? OperatingExpensesFFDRPFundAmt { get; set; }
        public decimal? OperatingExpensesEligibleCost { get; set; }
        public decimal? TravelExpensesCACFundAmt { get; set; }
        public decimal? TravelExpensesFFDRPFundAmt { get; set; }
        public decimal? TravelExpensesEligibleCost { get; set; }
        public decimal? TOTALCACFundAmt { get; set; }
        public decimal? TOTALFFDRPFundAmt { get; set; }
        public decimal? TOTELEligibleCost { get; set; }
        public decimal? CaliforniaEligibilityRate { get; set; }
        public decimal? CaliforniaEligibilityAmt { get; set; }
        public decimal? FederalReimbursementRate { get; set; }
        public decimal? FederalReimbursementAmt { get; set; }
        public decimal? TOTALReimbursementForMonth { get; set; }
        public decimal? AdjustmentAmount { get; set; }
        public decimal? TOTALAdjustedReimbursementForMonth { get; set; }
        public string PreparedByName { get; set; }
        public string PreparedByTitle { get; set; }
        public string PreparedByPhone { get; set; }
        public string PreparedByEmail { get; set; }
        public string Note { get; set; }

        public int? ReadOnly { get; set; }
        public decimal? TrainingEligibleAmount { get; set; }
        public decimal? TrainingEligibleRateCA { get; set; } 
        public decimal? TrainingEligibleAmtCA { get; set; }
        public decimal? TrainingReimbursementRateFed { get; set; }
        public decimal? TrainingReimbursementAmt { get; set; }
        public decimal? TrainingNonAttorneyAmt { get; set; }
        public string DisplayTitleIVeInvoiceID { get; set; }
        public string StandardAgreementNumber { get; set; }



    }
}
