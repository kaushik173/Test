using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.GoogleDrive
{
    public class NG_GoogleDriveAPICredentialByAgencyID_spParams
    {
        public int AgencyID { get; set; }
        public string AccountType { get; set; }

    }
    public class NG_GoogleDriveAPICredentialByAgencyID_spResult
    {
        public int GoogleDriveCredentialID { get; set; }
        public string AccountType { get; set; }
        public string APIAccountEmail { get; set; }

        public string APIKeyFilePath { get; set; }
        public string GoogleDriveID_TEST { get; set; }
        public string GoogleDriveID_PROD { get; set; }

        public string GoogleDriveTestFolderID
        {
            get
            {
                return GoogleDriveID_TEST;

            }
            set
            {
                GoogleDriveID_TEST = value;
            }
        }
        public string GoogleDriveProdFolderID
        {
            get
            {
                return GoogleDriveID_PROD;

            }
            set
            {
                GoogleDriveID_PROD = value;
            }
        }





    }
}
