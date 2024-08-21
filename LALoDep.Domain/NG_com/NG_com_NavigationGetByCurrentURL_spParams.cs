using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.NG_com
{
   public class NG_com_NavigationGetByCurrentURL_spParams
    {
        public string CurrentURL { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
