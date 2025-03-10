using XYZ.DataAccess.Tables.Base;

namespace XYZ.DataAccess.Tables.ORDER_TABLE
{
    /// <summary>
    /// ORDER table representation in database, contains all needed base order information, POCO
    /// </summary>
    public class ORDER : TABLE_BASE
    {
        /// <summary>
        /// Order completion status.
        /// </summary>
        public int ORDER_STATUS { get; set; }

        /// <summary>
        /// Order number (not main Id);
        /// </summary>
        public long ORDER_NUMBER { get; set; }

        /// <summary>
        /// FK to ID in USER table.
        /// </summary>
        public long USER_ID { get; set; }

        /// <summary>
        /// Amount that was paid for the order.
        /// </summary>
        public decimal PAYABLE_AMOUNT { get; set; }

        /// <summary>
        /// Optional order description.
        /// </summary>
        public string? DESCRIPTION { get; set; }

        /// <summary>
        /// Paysera gateway order id, filled if paysera is gateway for this order.
        /// </summary>
        public long? PAYSERA_ORDER_ID { get; set; }

        /// <summary>
        /// Paypal gateway order id, filled if paypal is gateway for this order.
        /// </summary>
        public long? PAYPAL_ORDER_ID { get; set; }
    }
}
