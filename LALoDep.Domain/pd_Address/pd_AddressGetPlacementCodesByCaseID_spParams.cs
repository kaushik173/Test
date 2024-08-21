using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Address
{
    public class pd_AddressGetPlacementCodesByCaseID_spParams
    {
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int? AgencyID { get; set; }

    }
    public class pd_AddressGetPlacementCodesByCaseID_spResult
    {
        public int? AddressID { get; set; }
        public int? AgencyID { get; set; }
        public string AddressStreet { get; set; }
        public string AddressCity { get; set; }
        public int? AddressStateCodeID { get; set; }
        public string AddressZipCode { get; set; }
        public int? AddressCountryCodeID { get; set; }
        public string AddressHomePhone { get; set; }

        public int? PlacementAgencyCodeID { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PlacementAgency { get; set; }
        public int? LFH { get; set; }
    }
    public class pd_PersonAddressGetAllRolesByCaseID_spParams
    {
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class pd_PersonAddressGetAllRolesByCaseID_spResult
    {
        public int? PersonAddressTypeCodeID { get; set; }
        public System.DateTime? PersonAddressStartDate { get; set; }
        public System.DateTime? PersonAddressEndDate { get; set; }
        public string PersonAddressWorkPhone { get; set; }
        public int? PersonAddressID { get; set; }
        public int? AddressID { get; set; }
        public int? AgencyID { get; set; }
        public int? PersonID { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameLast { get; set; }
        public short? RecordStateID { get; set; }

        public int? RoleTypeCodeID { get; set; }
        public string RoleType { get; set; }
        public string AddressType { get; set; }
        public string AddressStreet { get; set; }
        public string PlacementAgency { get; set; }
        public byte? RoleClient { get; set; }
        public string AddressCity { get; set; }
        public string AddressZipCode { get; set; }
        public int? AddressStateCodeID { get; set; }
        public int? AddressCountryCodeID { get; set; }
        public string AddressHomePhone { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public int SystemValueSequence { get; set; }
        public System.DateTime? PersonAddresssDateSort { get; set; }
        public string NameDisplay { get; set; }
        public string ContactDisplay { get; set; }
     

        public string LivesWith { get; set; }
    }
    public class pd_AddressGetByCaseID_spParams
    {
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class pd_AddressGetByCaseID_spResult
    {

        public int AddressID { get; set; }
        public int? AgencyID { get; set; }
        public string AddressStreet { get; set; }
        public string AddressCity { get; set; }
        public int? AddressStateCodeID { get; set; }
        public int? AddressCountryCodeID { get; set; }
        public string AddressZipCode { get; set; }
        public string AddressHomePhone { get; set; }
        public short RecordStateID { get; set; }

        public string State { get; set; }
        public string Country { get; set; }
        public int? PlacementAgencyCodeID { get; set; }
        public string PlacementAgency { get; set; }
    }
    public class pd_PlacementAddressGetByPlacementAgencyCodeID_spResult
    {

        public int AddressID { get; set; }
        public string AddressStreet { get; set; }
        public string AddressCity { get; set; }
        public int? AddressStateCodeID { get; set; }
        public string AddressZipCode { get; set; }
        public int? AddressCountryCodeID { get; set; }
        public string AddressHomePhone { get; set; }

    }
    public class pd_PlacementAddressGetByPlacementAgencyCodeID_spParams
    {
        public int PlacementAgencyCodeID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class pd_AddressGet_spParams
    {
        public int AddressID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
 
    public class pd_AddressGet_spResult
    {

        public int AddressID { get; set; }
        public int AgencyID { get; set; }
        public string AddressStreet { get; set; }
        public string AddressCity { get; set; }
        public int? AddressStateCodeID { get; set; }
        public int? AddressCountryCodeID { get; set; }
        public string AddressZipCode { get; set; }
        public string AddressHomePhone { get; set; }
        public short RecordStateID { get; set; }

        public string State { get; set; }
        public string Country { get; set; }
        public byte? CanTextFlag { get; set; }
    }


    public class pd_AddressUpdate_spParams
    {

        public int AddressID { get; set; }
        public int AgencyID { get; set; }
        public string AddressStreet { get; set; }
        public string AddressCity { get; set; }
        public int AddressStateCodeID { get; set; }
        public int AddressCountryCodeID { get; set; }
        public string AddressZipCode { get; set; }
        public string AddressHomePhone { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }

        public byte? CanTextFlag { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }


    public class pd_AddressInsert_spParams
    {

        public int AddressID { get; set; }
        public int AgencyID { get; set; }
        public string AddressStreet { get; set; }
        public string AddressCity { get; set; }
        public int AddressStateCodeID { get; set; }
        public int AddressCountryCodeID { get; set; }
        public string AddressZipCode { get; set; }
        public string AddressHomePhone { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }

        public byte? CanTextFlag { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class pd_PersonAddressInsert_spParams
    {


        public int PersonAddressID { get; set; }
        public int AgencyID { get; set; }
        public int PersonID { get; set; }
        public int AddressID { get; set; }
        public int PersonAddressTypeCodeID { get; set; }
        public string PersonAddressWorkPhone { get; set; }
        public short PersonAddressConfidential { get; set; }
        public DateTime PersonAddressStartDate { get; set; }
        public DateTime? PersonAddressEndDate { get; set; }

        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }


        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

        public string AddressTypeEnumName { get; set; }



    }

    public class pd_PlacementAgencyAddressInsert_spParams
    {


        public int  PlacementAgencyAddressID { get; set; }
        public int AgencyID { get; set; }
        public int PlacementAgencyCodeID { get; set; }
        public int AddressID { get; set; }
       

        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }


        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

     


    }
    
    
   public class pd_PersonContactInsert_spParams
    {
        public int PersonContactID { get; set; }
        public int? AgencyID { get; set; }
        public int PersonID { get; set; }
        public int PersonContactTypeCodeID { get; set; }
        public string PersonContactInfo { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }


        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

        public string ContactTypeEnumName { get; set; }



    }public class pd_PersonContactInsert_spResult
    {
        public decimal PersonContactID { get; set; }

        public decimal InsertedID { get; set; }
       


    }
    

    public class pd_PersonAddressUpdate_spParams
    {


        public int PersonAddressID { get; set; }
        public int AgencyID { get; set; }
        public int PersonID { get; set; }
        public int AddressID { get; set; }
        public int PersonAddressTypeCodeID { get; set; }
        public string PersonAddressWorkPhone { get; set; }
        public short PersonAddressConfidential { get; set; }
        public DateTime PersonAddressStartDate { get; set; }
        public DateTime? PersonAddressEndDate { get; set; }

        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }


        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }





    }
    public class pd_PersonAddressDelete_spParams
    {


        public int ID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

        public string RecordTimeStamp { get; set; }






    }
}