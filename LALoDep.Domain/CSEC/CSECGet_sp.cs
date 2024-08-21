using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.CSEC
{
    public class CSECGet_spParams
    {
        public int CSECID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class CSECGet_spResult
    {
        public int CSECID { get; set; }
        public int CaseID { get; set; }
        public string Child { get; set; }
        public string DueDate { get; set; }
        public string CompletionDate { get; set; }
        public string AssignedTo { get; set; }
        public string CSECNote { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("1A")]
        public short? Ans1A { get; set; }
        public short? Ans1B { get; set; }
        public short? Ans1C { get; set; }
        public short? Ans1D { get; set; }
        public short? Ans1E { get; set; }
        public short? Ans1F { get; set; }
        public short? Ans1G { get; set; }
        public short? Ans2A { get; set; }
        public short? Ans2B { get; set; }
        public short? Ans2C { get; set; }
        public short? Ans2D { get; set; }
        public short? Ans3A { get; set; }
        public short? Ans3B { get; set; }
        public short? Ans3C { get; set; }
        public short? Ans3D { get; set; }
        public short? Ans3E { get; set; }
        public short? Ans3F { get; set; }
        public short? Ans3G { get; set; }
        public short? Ans3H { get; set; }
        public short? Ans4A { get; set; }
        public short? Ans4B { get; set; }
        public short? Ans4C { get; set; }
        public short? Ans4D { get; set; }
        public short? Ans4E { get; set; }
        public short? Ans4F { get; set; }
        public short? Ans4G { get; set; }
        public short? Ans5A { get; set; }
        public short? Ans5B { get; set; }
        public short? Ans5C { get; set; }
        public short? Ans5D { get; set; }
        public short? Ans5E { get; set; }
        public short? Ans5F { get; set; }
        public short? Ans6A { get; set; }
        public short? Ans6B { get; set; }
        public short? Ans6C { get; set; }
        public short? Ans6D { get; set; }
        public short? Ans7A { get; set; }
        public short? Ans7B { get; set; }
        public short? Ans7C { get; set; }
        public short? Ans7D { get; set; }
        public short? Ans7E { get; set; }
        public short? Ans7F { get; set; }
        public short? Ans8A { get; set; }
        public short? Ans8B { get; set; }
        public short? Ans8C { get; set; }
        public short? Ans8D { get; set; }

    }


    public class   CSECGetForEmail_spParams
    {
        public int CSECID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class   CSECGetForEmail_spResult
{
        public string AlertTitle { get; set; }
        public string AlertMessage { get; set; }
        public int SendEmailFlag { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }


    }
}
