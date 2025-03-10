using XYZ.DataAccess.Tables.Base;

namespace XYZ.DataAccess.Tables.USER_ERROR_TABLE
{
    /// <summary>
    /// USER_ERROR table representation in database, contains errors specific to user, POCO
    /// </summary>
    public class USER_ERROR : TABLE_BASE
    {
        /// <summary>
        /// FK to main user identifier in USER table
        /// </summary>
        public long USER_ID { get; set; }

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
