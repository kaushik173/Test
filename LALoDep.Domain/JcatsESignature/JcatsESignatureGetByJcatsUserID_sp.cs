namespace LALoDep.Domain.JcatsESignature
{
    public class JcatsESignatureGetByJcatsUserID_spParams {
  public int? JcatsUserID { get; set; }
  public int? UserID { get; set; }

}
    public class JcatsESignatureDelete_spParams
    {
        public int? ID { get; set; }
        public int? UserID { get; set; }

    }

    public class JcatsESignatureGetByJcatsUserID_spResult
	{
		public JcatsESignatureGetByJcatsUserID_spResult()
		{
		}
		public int? JcatsUserID	{ get; set; }
		public string FileRootPath	{ get; set; }
		public string NewFilePath	{ get; set; }
		public string NewFileName	{ get; set; }
		public int? JcatsESignatureID	{ get; set; }
		public string SignatureFilePath	{ get; set; }
		public string InitialsFilePath	{ get; set; }
		public byte? ES_RecordStateID	{ get; set; }
		public decimal? ES_RecordTimeStamp	{ get; set; }
	}
}
