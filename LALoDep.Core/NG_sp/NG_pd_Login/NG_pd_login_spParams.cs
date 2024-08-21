using System;

namespace LALoDep.Core.NG_pd_login_spParams
{
    public class NG_pd_login_spParams
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public byte AdminFlag { get; set; }
        public Guid LoginKey { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int AvailWidth { get; set; }
        public int AvailHeight { get; set; }
        public string ClientInfo { get; set; }
        public string ClientAddress { get; set; }
        public string ClientHost { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int RecordStateID { get; set; }
        public string ServerInfo  { get; set; }
    }
    public class NG_pd_LoginCheckSuccess_spParams
    {
        
        
            
       
        public string LoginKey { get; set; }
      
      
        public int RecordStateID { get; set; }
        public int  UserID { get; set; }
        public Guid  BatchLogJobID { get; set; }
        public int AdminFlag { get; set; }
        public string ClientAddress { get; set; }
        public string ClientHost { get; set; }
        public string ASPPageName { get; set; }
         public string ServerInfo { get; set; }
       public short LogoutFlag { get; set; }
        public short ReturnRecordFlag { get; set; }

    }
  

}