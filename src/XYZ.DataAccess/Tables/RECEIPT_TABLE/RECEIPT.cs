using XYZ.DataAccess.Tables.Base;

namespace XYZ.DataAccess.Tables.RECEIPT_TABLE
{
    public class RECEIPT : TABLE_BASE
    {
        public long USER_ID { get; set; }

        public string RECEIPT_ID { get; set; }

        public long ORDER_NUMBER { get; set; }

        public string GATEWAY_TRANSACTION_ID { get; set; }
    }
}
