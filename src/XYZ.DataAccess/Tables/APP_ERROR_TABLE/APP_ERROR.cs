using XYZ.DataAccess.Tables.Base;

namespace XYZ.DataAccess.Tables.APP_ERROR_TABLE
{
    /// <summary>
    /// APP_ERROR table representation in database, contains errors that occured in application (unhanled exceptions mostly), POCO
    /// </summary>
    public class APP_ERROR : TABLE_BASE
    {
        /// <summary>
        /// Exception message
        /// </summary>
        public string MESSAGE { get; set; }

        /// <summary>
        /// Exception stacktrace
        /// </summary>
        public string STACKTRACE { get; set; }

        /// <summary>
        /// Additional helpful text to help track error cause.
        /// </summary>
        public string? ADDITIONAL_TEXT { get; set; }
    }
}
