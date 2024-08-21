using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models
{
    public class TreeViewModel
    {
        public TreeViewModel()
        {

        }
        public int id;
        public string name;
        public string type;
        public string pageUrl;
        public int selectedId;
        public List<TreeViewModel> children;
    }
}