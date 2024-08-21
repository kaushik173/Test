using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.BatchHearing
{
    public class pd_BatchHearingSearch_spParams
    {
        public string DocketNumber { get; set; }
        public string DocketNumber2   {get;set;}
        public string DocketNumber3   {get;set;}
        public string DocketNumber4   {get;set;}
        public string DocketNumber5   {get;set;}
        public string DocketNumber6   {get;set;}
        public int UserID          {get;set;}
        public Guid BatchLogJobID   {get;set;}
    }
}
