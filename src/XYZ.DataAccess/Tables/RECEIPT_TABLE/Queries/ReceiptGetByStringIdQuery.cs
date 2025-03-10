using XYZ.DataAccess.Base;
using XYZ.DataAccess.Interfaces;

namespace XYZ.DataAccess.Tables.RECEIPT_TABLE.Queries
{
    /// <summary>
    /// Gets receipt information by main identifier.
    /// </summary>
    public class ReceiptGetByStringIdQuery : QueryBase<RECEIPT>, IQuery<RECEIPT?>
    {
        /// <summary>
        /// Main identifier.
        /// </summary>
        private string _id;

        /// <summary>
        /// Gets receipt information by main identifier.
        /// </summary>
        public ReceiptGetByStringIdQuery(string id)
        {
            _id = id;
        }

        public override string SqlQuery =>
            $@"
                SELECT 
                    {TableAllParams}
                FROM {_tbl.Scheme}.{_tbl.TableName}
                WHERE {nameof(_tbl.RECEIPT_ID)} = @{nameof(_id)}";

        public async Task<RECEIPT?> ExecuteAsync(IDatabaseQueryExecutionLogic databaseQueryExecutionLogic)
        {
            object parameters = new
            {
                _id = _id
            };
            return await databaseQueryExecutionLogic.QueryFirstOrDefaultAsync<RECEIPT?>(SqlQuery, parameters);
        }
    }
}
