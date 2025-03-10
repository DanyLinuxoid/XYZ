using XYZ.DataAccess.Base;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.USER_TABLE;

namespace XYZ.DataAccess.Tables.USER_TBL.Queries
{
    /// <summary>
    /// Gets user by main identifier.
    /// </summary>
    public class UserGetByIdQuery : QueryBase<USER>, IQuery<USER?>
    {
        /// <summary>
        /// Main identifier.
        /// </summary>
        private long _id;

        /// <summary>
        /// Gets user by main identifier.
        /// </summary>
        public UserGetByIdQuery(long id)
        {
            _id = id;
        }

        public override string SqlQuery =>
        $@"
            SELECT 
                {TableAllParams}
            FROM {_tbl.Scheme}.{_tbl.TableName}
            WHERE {nameof(_tbl.ID)} = @{nameof(_id)}";

        public async Task<USER?> ExecuteAsync(IDatabaseQueryExecutionLogic databaseQueryExecutionLogic)
        {
            object parameters = new
            {
                _id = _id,
            };
            return await databaseQueryExecutionLogic.QueryFirstOrDefaultAsync<USER?>(SqlQuery, parameters);
        }
    }
}
