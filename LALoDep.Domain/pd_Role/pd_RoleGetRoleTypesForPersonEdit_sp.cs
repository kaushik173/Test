namespace LALoDep.Domain.pd_Role
{
    public class pd_RoleGetRoleTypesForPersonEdit_spParams
    {
        public int? RoleID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class pd_RoleGetRoleTypesForPersonEdit_spResult
    {
        public pd_RoleGetRoleTypesForPersonEdit_spResult()
        {
        }
        public string CodeDisplay { get; set; }
        public int? CodeID { get; set; }
    }
}
