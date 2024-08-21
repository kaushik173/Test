using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LALoDep.Core.Custom.Utility;

namespace LALoDep.Models
{
    public class CodeViewModel
    {
        public int Selected { get; set; }
        public int CodeID { get; set; }
        public int CodeTypeID { get; set; }
        public string CodeValue { get; set; }
        public string CodeShortValue { get; set; }
        public string EncryptedCodeID { get { return Utility.Encrypt(CodeID.ToString()); } }
        public string CodeDisplay { get; set; }
    }
}