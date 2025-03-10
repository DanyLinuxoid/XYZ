using XYZ.DataAccess.Base;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.ORDER_TABLE;

namespace XYZ.DataAccess.Tables.ORDER_TBL.Queries
{
    /// <summary>
    /// Gets order related to user by order number and user main identifier.
    /// </summary>
    public class OrderByOrderNumberAndUserIdGetQuery : QueryBase<ORDER>, IQuery<ORDER?>
    {
        /// <summary>
        /// User main identifier.
        /// </summary>
        private long _userId;

        /// <summary>
        /// Order number (not main id).
        /// </summary>
        private long _orderNumber;

        /// <summary>
        /// Gets order related to user by order number and user main identifier.
        /// </summary>
        public OrderByOrderNumberAndUserIdGetQuery(long userId, long orderNumber)
        {
            _userId = userId;
            _orderNumber = orderNumber;
        }

        public override string SqlQuery =>
        $@"
            SELECT 
                {TableAllParams}
            FROM {_tbl.Scheme}.{_tbl.TableName}
            WHERE {nameof(_tbl.ORDER_NUMBER)} = @{nameof(_orderNumber)}
                AND {nameof(_tbl.USER_ID)} = @{nameof(_userId)}";

        public async Task<ORDER?> ExecuteAsync(IDatabaseQueryExecutionLogic databaseQueryExecutionLogic)
        {
            object parameters = new
            {
                _userId = _userId,
                _orderNumber = _orderNumber,
            };
            return await databaseQueryExecutionLogic.QueryFirstOrDefaultAsync<ORDER?>(SqlQuery, parameters);
        }
    }
}
