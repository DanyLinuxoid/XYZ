using XYZ.DataAccess.Tables.Base;

namespace XYZ.DataAccess.Tables.USER_ERROR_TABLE
{
    public class USER_ERROR : TABLE_BASE
    {
        public long USER_ID { get; set; }

        public string MESSAGE { get; set; }

        public string STACKTRACE { get; set; }

        public string ADDITIONAL_TEXT { get; set; }
    }
}
