using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Association
{
    public class pd_AssociationGetByCaseID_spParams
    {
        public int CaseID { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_AssociationGetByCaseID_spResult
    {
        public int AssociationID { get; set; }
        public int AgencyID { get; set; }
        public int CaseID { get; set; }
        public int PersonID { get; set; }
        public int RelatedPersonID { get; set; }
        public int AssociationCodeID { get; set; }
        public System.DateTime? AssociationStartDate { get; set; }
        public System.DateTime? AssociationEndDate { get; set; }
        public short RecordStateID { get; set; }
        public string Association { get; set; }
        public string AssociationShort { get; set; }
        public string PersonLastName { get; set; }
        public string PersonFirstName { get; set; }
        public string RelatedPersonLastName { get; set; }
        public string RelatedPersonFirstName { get; set; }
        public byte PersonClient { get; set; }
        public byte RelatedPersonClient { get; set; }
        public string Role { get; set; }
        public int Sort { get; set; }
    }

    public class pd_AssociationInsert_spParams
    {
        public int AssociationID { get; set; }
        public int AgencyID { get; set; }
        public int CaseID { get; set; }
        public int PersonID { get; set; }
        public int RelatedPersonID { get; set; }
        public int AssociationCodeID { get; set; }
        public DateTime AssociationStartDate { get; set; }
        public DateTime? AssociationEndDate { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }


        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_AssociationUpdateStartDateEndDate_spParams
    {
        public int AssociationID { get; set; }
    
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }


        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }


    public class pd_AssociationDelete_spParams
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

        public string RecordTimeStamp { get; set; }
    }
    public class pd_AssociationSuggestByCaseID_spParams
    {
        public int CaseID { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_AssociationSuggestByCaseID_spResult
    {
        public int? Role1ID { get; set; }
        public int? Role1TypeID { get; set; }
        public string Role1Type { get; set; }
        public byte? Client1 { get; set; }
        public string FirstName1 { get; set; }
        public string LastName1 { get; set; }
        public int? Person1SexID { get; set; }
        public int? PersonID1 { get; set; }
        public int? Role2ID { get; set; }
        public int? Role2TypeID { get; set; }
        public byte? Client2 { get; set; }
        public string Role2Type { get; set; }
        public string FirstName2 { get; set; }
        public string LastName2 { get; set; }
        public int? Person2SexID { get; set; }
        public int? PersonID2 { get; set; }
        public int? SuggestedAssociationID { get; set; }
    }

 








}
