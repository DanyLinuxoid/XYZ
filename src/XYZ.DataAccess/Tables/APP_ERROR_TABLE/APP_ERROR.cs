using XYZ.DataAccess.Tables.Base;

namespace XYZ.DataAccess.Tables.APP_ERROR_TABLE
{
    public class APP_ERROR : TABLE_BASE
    {
        public string MESSAGE { get; set; }

        public string STACKTRACE { get; set; }

        public string ADDITIONAL_TEXT { get; set; }
    }
}
