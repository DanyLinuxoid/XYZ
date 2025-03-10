namespace XYZ.DataAccess.Tables.Base
{
    /// <summary>
    /// Base rows that should be in every receord/row. 
    /// </summary>
    public class TABLE_RECORDS_BASE
    {
        /// <summary>
        /// Main table identifier.
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Creation date for record.
        /// </summary>
        public DateTime DB_RECORD_CREATION_TIME { get; set; }

        /// <summary>
        /// Update date for record.
        /// </summary>
        public DateTime? DB_RECORD_UPDATE_TIME { get; set; }

        /// <summary>
        /// Mappings to use in logic for auto-mapping.
        /// </summary>
        public Dictionary<string, object?> TableEntriesBase =>
            new Dictionary<string, object?>()
            {
                [nameof(ID)] = ID,
                [nameof(DB_RECORD_CREATION_TIME)] = DB_RECORD_CREATION_TIME,
                [nameof(DB_RECORD_UPDATE_TIME)] = DB_RECORD_UPDATE_TIME,
            };
    }
}
