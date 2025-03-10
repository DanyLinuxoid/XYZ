using XYZ.Models.Features.Billing.Data;
using XYZ.Models.Features.Billing.Data.Dto;

namespace XYZ.Logic.Common.Interfaces
{
    public interface IReceiptLogic
    {
        Task<ReceiptDto?> GetReceipt(string receiptId);
        Task<long> SaveReceipt(ReceiptDto? receipt);
    }
}