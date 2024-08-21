using System.Collections.Generic;

namespace LALoDep.Models
{
    public class MenuItem
    {
        public MenuItem() 
        {
            Items = new List<MenuItem>();
        }

        public string Title { get; set; }
        public string Tooltip { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public List<MenuItem> Items { get; set; }
    }
}