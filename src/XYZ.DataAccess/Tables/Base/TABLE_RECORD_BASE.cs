namespace XYZ.DataAccess.Tables.Base
{
    public class TABLE_RECORDS_BASE
    {
        public long ID { get; set; }

        public DateTime DB_RECORD_CREATION_TIME { get; set; }

        public DateTime? DB_RECORD_UPDATE_TIME { get; set; }

        public Dictionary<string, object?> TableEntriesBase =>
            new Dictionary<string, object?>()
            {
                [nameof(ID)] = ID,
                [nameof(DB_RECORD_CREATION_TIME)] = DB_RECORD_CREATION_TIME,
                [nameof(DB_RECORD_UPDATE_TIME)] = DB_RECORD_UPDATE_TIME,
            };
    }
}
