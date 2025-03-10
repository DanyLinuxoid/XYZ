namespace XYZ.DataAccess.Tables.Base
{
    public class ERROR_BASE : TABLE_BASE
    {
        public string ERROR_MESSAGE { get; set; }

        public string STACKTRACE { get; set; }

        public string ADDITIONAL_TEXT { get; set; }
    }
}
