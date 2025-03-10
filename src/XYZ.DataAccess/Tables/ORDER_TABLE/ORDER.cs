using XYZ.DataAccess.Tables.Base;

namespace XYZ.DataAccess.Tables.ORDER_TABLE
{
    public class ORDER : TABLE_BASE
    {
        public int ORDER_STATUS { get; set; }

        public long ORDER_NUMBER { get; set; }

        public long USER_ID { get; set; }

        public decimal PAYABLE_AMOUNT { get; set; }

        public string? DESCRIPTION { get; set; }

        public long? PAYSERA_ORDER_ID { get; set; }

        public long? PAYPAL_ORDER_ID { get; set; }
    }
}
