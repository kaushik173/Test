//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LALoDep.Domain
{
    using System;
    
    public partial class pd_PersonGet_sp_Result
    {
        public int PersonID { get; set; }
        public int AgencyID { get; set; }
        public Nullable<System.DateTime> PersonDOB { get; set; }
        public Nullable<int> PersonRaceCodeID { get; set; }
        public Nullable<int> PersonSexCodeID { get; set; }
        public short RecordStateID { get; set; }
        public byte[] RecordTimeStamp { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Sex { get; set; }
        public string Race { get; set; }
        public string Email { get; set; }
        public int PersonNameID { get; set; }
        public string Language { get; set; }
        public int PersonNameTypeCodeID { get; set; }
        public Nullable<int> PersonLanguageCodeID { get; set; }
        public Nullable<int> RoleAgencyID { get; set; }
        public Nullable<System.DateTime> DeceasedDate { get; set; }
        public Nullable<int> DeceasedDate_PersonClassID { get; set; }
    }
}
