using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.Base;
using XYZ.Logic.Common.Interfaces;

namespace XYZ.Logic.Features.Billing.Common
{
    /// <summary>
    /// Gateway specific order saving logic.
    /// </summary>
    public class GatewayOrderSavingLogic<T> : IGatewayOrderSavingLogic<T> where T : TABLE_BASE, new()
    {
        /// <summary>
        /// Database access.
        /// </summary>
        private IDatabaseLogic _databaseLogic;

        /// <summary>
        /// Repo to use for update
        /// </summary>
        private ICommandRepository<T> _commandRepository;

        /// <summary>
        /// Gateway specific order saving constructor.
        /// </summary>
        /// <param name="databaseLogic">Database access.</param>
        public GatewayOrderSavingLogic(IDatabaseLogic databaseLogic, ICommandRepository<T> commandRepository)
        {
            _databaseLogic = databaseLogic;
            _commandRepository = commandRepository;
        }

        /// <summary>
        /// Saves gateway specific order.
        /// </summary>
        /// <returns>Saved order main identifier.</returns>
        public async Task<long> SaveOrder(T model)
        {
            return await _databaseLogic.CommandAsync(_commandRepository, CommandTypes.Create, model);
        }
    }
}
