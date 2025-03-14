﻿using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.RECEIPT_TABLE;
using XYZ.DataAccess.Tables.RECEIPT_TABLE.Queries;
using XYZ.Logic.Common.Interfaces;
using XYZ.Logic.Features.Billing.Mappers;
using XYZ.Models.Features.Billing.Data.Dto;

namespace XYZ.Logic.Features.Billing.Common
{
    /// <summary>
    /// Main receipt logic class for receipt CRUD's.
    /// </summary>
    public class ReceiptLogic : IReceiptLogic
    {
        /// <summary>
        /// Database access.
        /// </summary>
        private IDatabaseLogic _databaseLogic;

        /// Main receipt logic constructor for receipt CRUD's.
        public ReceiptLogic(IDatabaseLogic databaseLogic)
        {
            _databaseLogic = databaseLogic;
        }

        /// <summary>
        /// Gets receipt by receipt string identifier (not main).
        /// </summary>
        /// <param name="receiptId">Receipt string identifier that was generated by server.</param>
        /// <returns>Receipt result if found, null otherwise.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ReceiptDto?> GetReceipt(string receiptId)
        {
            if (string.IsNullOrEmpty(receiptId))
                throw new ArgumentNullException(nameof(receiptId));

            RECEIPT? result = await _databaseLogic.QueryAsync(new ReceiptGetByStringIdQuery(receiptId));
            return result == null ? null : result.ToDto();
        }

        /// <summary>
        /// Main receipt saving logic.
        /// </summary>
        /// <param name="receipt">Order receipt.</param>
        /// <returns>Saved receipt main identifier.</returns>
        /// <exception cref="ArgumentNullException">Thrown if parameter is null</exception>
        public async Task<long> SaveReceipt(ReceiptDto? receipt)
        {
            if (receipt == null)
                throw new ArgumentNullException(nameof(receipt));

            return await _databaseLogic.CommandAsync(new RECEIPT_CUD(), CommandTypes.Create, receipt.ToDbo());
        }
    }
}
