using XYZ.DataAccess.Tables.RECEIPT_TABLE;
using XYZ.Models.Features.Billing.Data.Dto;

namespace XYZ.Logic.Features.Billing.Mappers
{
    public static class ReceiptMapper
    {
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