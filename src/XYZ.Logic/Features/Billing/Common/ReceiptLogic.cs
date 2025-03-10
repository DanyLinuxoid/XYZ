using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.RECEIPT_TABLE;
using XYZ.DataAccess.Tables.RECEIPT_TABLE.Queries;
using XYZ.Logic.Common.Interfaces;
using XYZ.Logic.Features.Billing.Mappers;
using XYZ.Models.Features.Billing.Data.Dto;

namespace XYZ.Logic.Features.Billing.Common
{
    public class ReceiptLogic : IReceiptLogic
    {
        private IDatabaseLogic _databaseLogic;

        public ReceiptLogic(IDatabaseLogic databaseLogic)
        {
            _databaseLogic = databaseLogic;
        }

        public async Task<ReceiptDto?> GetReceipt(string receiptId)
        {
            if (string.IsNullOrEmpty(receiptId))
                throw new ArgumentNullException(nameof(receiptId));

            RECEIPT? result = await _databaseLogic.QueryAsync(new ReceiptGetByStringIdQuery(receiptId));
            return result == null ? null : result.ToDto();
        }

        public async Task<long> SaveReceipt(ReceiptDto? receipt)
        {
            if (receipt == null)
                throw new ArgumentNullException(nameof(receipt));

            return await _databaseLogic.CommandAsync(new RECEIPT_CUD(), CommandTypes.Create, receipt.ToDbo());
        }
    }
}
