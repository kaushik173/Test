
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.JcatsESignature
{



    public class JcatsESignatureUpdate_spParams
    {
        public int? JcatsESignatureID { get; set; }
        public int? JcatsUserID { get; set; }
        public string SignatureFilePath { get; set; }
        public string InitialsFilePath { get; set; }
        public int? UserID { get; set; }

    }

}