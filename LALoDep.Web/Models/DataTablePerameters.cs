using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models
{
    public class DataTablePerameters
    {
        /// <summary>
        /// Request sequence number sent by DataTable,
        /// same value must be returned in response
        /// </summary>       
        public string sEcho { get; set; }

        /// <summary>
        /// Text used for filtering
        /// </summary>
        public string sSearch { get; set; }

        /// <summary>
        /// Number of records that should be shown in table
        /// </summary>
        public int iDisplayLength { get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int iDisplayStart { get; set; }

        /// <summary>
        /// Number of columns in table
        /// </summary>
        public int iColumns { get; set; }

        /// <summary>
        /// Column number for sort
        /// </summary>
        public int iSortCol_0 { get; set; }
        /// <summary>
        /// Direction of sorting
        /// </summary>
        public string sSortDir_0 { get; set; }
        public bool IsDescSort { get { return this.sSortDir_0 == "desc"; } }

        public string filterType { get; set; }
    }
}