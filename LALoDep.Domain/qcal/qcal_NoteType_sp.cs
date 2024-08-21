using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.qcal
{

    public class qcal_NoteType_spParams
    {


        public string LoadOption { get; set; }
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }

    }
    public class qcal_NoteType_spResult
    {
        public int CodeID { get; set; }
        public string CodeDisplay { get; set; }

    }
    public class qcal_HearingType_spParams
    {


        public string LoadOption { get; set; }
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }

    }
    public class qcal_HearingType_spResult
    {
        public int CodeID { get; set; }
        public string CodeDisplay { get; set; }

    }
    public class qcal_ClientType_spParams
    {


        public string LoadOption { get; set; }

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }

    }
    public class qcal_ClientType_spResult
    {
        public string ClientType { get; set; }
        public string ClientTypeDisplay { get; set; }

    }
    public class qcal_PredeterminedAnswersList_spParams
    {

        public int? AgencyGroupID { get; set; }
        public int? AgencyID { get; set; }
        public int? NoteTypeCodeID { get; set; }
        public int? HearingTypeCodeID { get; set; }
        public string ClientType { get; set; }
        public string LoadOption { get; set; }
        public int? HearingTypeGroupCodeID { get; set; }
        

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }

    }
    public class qcal_PredeterminedAnswersList_spResult
    {
        public int? InactiveFlag { get; set; }
        public int? Seq { get; set; }
        public string PredeterminedAnswer { get; set; }
        public string ShortValue { get; set; }
        public int? QuickNoteID { get; set; }
        public int? SortSeq { get; set; }

    }
    public class QuickNoteGetNoteTypes_spResult
    {
        public int CodeID { get; set; }
        public string CodeDisplay { get; set; }
        public int? QuickNoteCodeID { get; set; }
        public string QuickNoteCodeType { get; set; }

        public int? QuickNoteID { get; set; }

    }
    public class QuickNoteGetAgencies_spResult
    {
        public int AgencyID { get; set; }
        public string AgencyDisplay { get; set; }
        public int? QuickNoteAgencyID { get; set; }

        public int? QuickNoteID { get; set; }

    }
    public class QuickNoteGetHearingTypes_spResult
    {
        public int? GroupDisplayOrder { get; set; }
        public string GroupDisplay { get; set; }
        public int? GroupID { get; set; }
        public int CodeID { get; set; }
        public string CodeDisplay { get; set; }
        public int? QuickNoteCodeID { get; set; }
        public string QuickNoteCodeType { get; set; }
        public int? QuickNoteID { get; set; }
        
    }

    public class QuickNoteGet_spResult
    {


        public int QuickNoteID { get; set; }
        public string QuickNoteValue { get; set; }
        public string QuickNoteLongValue { get; set; }
        public int? QuickNoteChildClientFlag { get; set; }
        public int? QuickNoteAdultClientFlag { get; set; }
        public int? QuickNoteSequence { get; set; }
        public int? QuickNoteInactiveFlag { get; set; }
        public int? RecordStateID { get; set; }

    }

    public class QuickNoteGet_spParams
    {


        public int? QuickNoteID { get; set; }
        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }

    }
    public class QuickNoteIUD_spParams
    {


        public string IUD { get; set; }
        public int? QuickNoteID { get; set; }

        public string QuickNoteValue { get; set; }

        public string QuickNoteLongValue { get; set; }
        public int? QuickNoteChildClientFlag { get; set; }
        public int? QuickNoteAdultClientFlag { get; set; }
        public int? QuickNoteSequence { get; set; }
        public int? QuickNoteInactiveFlag { get; set; }
        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }

    }
    public class QuickNoteCodeIUD_spParams
    {


        public string IUD { get; set; }
        public int? QuickNoteID { get; set; }

        public string QuickNoteCodeType { get; set; }

        public int? QuickNoteCodeID { get; set; }
        public int? CodeID { get; set; }
        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }

    }
    public class QuickNoteAgencyIUD_spParams
    {


        public string IUD { get; set; }
        public int? QuickNoteID { get; set; }

        public int? QuickNoteAgencyID { get; set; }
        public int? AgencyID { get; set; }
        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }

    }
    public class qcal_CopyPredeterminedAnswers_spParams
    {


        public int? FromQuickNoteID { get; set; }

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }

    }

}


