using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Models.Sharepoint.CreateFolder 
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Metadata
    {
        public string id { get; set; }
        public string uri { get; set; }
        public string type { get; set; }
    }

    public class Deferred
    {
        public string uri { get; set; }
    }

    public class Files
    {
        public Deferred __deferred { get; set; }
    }

    public class ListItemAllFields
    {
        public Deferred __deferred { get; set; }
    }

    public class ParentFolder
    {
        public Deferred __deferred { get; set; }
    }

    public class Properties
    {
        public Deferred __deferred { get; set; }
    }

    public class StorageMetrics
    {
        public Deferred __deferred { get; set; }
    }

    public class Folders
    {
        public Deferred __deferred { get; set; }
    }

    public class D
    {
        public Metadata __metadata { get; set; }
        public Files Files { get; set; }
        public ListItemAllFields ListItemAllFields { get; set; }
        public ParentFolder ParentFolder { get; set; }
        public Properties Properties { get; set; }
        public StorageMetrics StorageMetrics { get; set; }
        public Folders Folders { get; set; }
        public bool Exists { get; set; }
        public bool IsWOPIEnabled { get; set; }
        public int ItemCount { get; set; }
        public string Name { get; set; }
        public object ProgID { get; set; }
        public string ServerRelativeUrl { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeLastModified { get; set; }
        public string UniqueId { get; set; }
        public string WelcomePage { get; set; }
    }

    public class CreateFolderResponse
    {
        public Error error { get; set; }
        public D d { get; set; }
    }
    public class Message
    {
        public string lang { get; set; }
        public string value { get; set; }
    }

    public class Error
    {
        public string code { get; set; }
        public Message message { get; set; }
    }

    public class CreateFolderRequest
    {
        public string ServerRelativeUrl { get; set; }
        
    }
}
