﻿using XYZ.DataAccess.Tables.RECEIPT_TABLE;
using XYZ.Models.Features.Billing.Data.Dto;

namespace XYZ.Logic.Features.Billing.Mappers
{
    /// <summary>
    /// Simple dto<->dbo mapping for receipts.
    /// </summary>
    public static class ReceiptMapper
    {
        /// <summary>
        /// Maps dbo receipt to dto.
        /// </summary>
        /// <param name="receipt">Dbo receipt.</param>
        /// <returns>Dto receipt.</returns>
        public static ReceiptDto ToDto(this RECEIPT receipt)
        {
            return new ReceiptDto() 
            {
                OrderNumber = receipt.ORDER_NUMBER,
                UserId = receipt.USER_ID,
                ReceiptId = receipt.RECEIPT_ID,
                GatewayTransactionId = receipt.GATEWAY_TRANSACTION_ID,
            };
        }

        /// <summary>
        /// Maps dtoreceipt to dbo for db saving.
        /// </summary>
        /// <param name="receipt">Dto receipt.</param>
        /// <returns>Dbo receipt.</returns>
        public static RECEIPT ToDbo(this ReceiptDto receipt)
        {
            return new RECEIPT() 
            {
                USER_ID = receipt.UserId,
                GATEWAY_TRANSACTION_ID = receipt.GatewayTransactionId,
                RECEIPT_ID = receipt.ReceiptId,
                ORDER_NUMBER = receipt.OrderNumber,
            };
        }
    }
}