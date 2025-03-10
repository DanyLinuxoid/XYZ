using XYZ.DataAccess.Helpers;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.Base;

namespace XYZ.DataAccess.Base
{
    public abstract class CommandsBase<T> where T : TABLE_BASE, new()
    {
        protected T _tbl { get; set; }

        /// <summary>
        /// Command to create record in database.
        /// </summary>
        protected virtual string _createCommand =>
            $@"INSERT INTO {_tbl.Scheme}.{_tbl.TableName} (

                {GetNormalStringFormattedParams(',')}

                {nameof(_tbl.DB_RECORD_CREATION_TIME)},
                {nameof(_tbl.DB_RECORD_UPDATE_TIME)}
            )
            OUTPUT INSERTED.Id
            VALUES(
                {GetNormalStringFormattedParams('@')}
                GETDATE(),
                GETDATE())";

        /// <summary>
        /// Command to update record in database.
        /// </summary>
        protected virtual string _updateCommand =>
            $@"UPDATE {_tbl.Scheme}.{_tbl.TableName}
            SET 
                {GetUpdateStringFormattedParams()},
                {nameof(_tbl.DB_RECORD_UPDATE_TIME)} = GETDATE()
            OUTPUT INSERTED.Id
            WHERE {nameof(_tbl.ID)} = @{nameof(_tbl.ID)}";

        /// <summary>
        /// Command to delete record from database.
        /// </summary>
        protected virtual string _deleteCommand =>
            $@"DELETE FROM {_tbl.Scheme}.{_tbl.TableName}
                WHERE {nameof(_tbl.ID)} = @{nameof(_tbl.ID)}";

        /// <summary>
        /// Async command to create record in database.
        /// </summary>
        /// <param name="dbo">Model to create.</param>
        /// <returns>Execution result which contains created row id or error message if failed.</returns>
        public virtual async Task<long> CreateAsync(T dbo, IDatabaseCommandExecutionLogic databaseCommandExecutionLogic)
        {
            dbo.FillTableInfo();
            _tbl = dbo;
            return await databaseCommandExecutionLogic.CreateAsync(_createCommand, CommandParametersHelper.GetDynamicParameters(MapParameters(dbo)));
        }

        /// <summary>
        /// Async command to create record in database.
        /// </summary>
        /// <param name="dbo">Model to update.</param>
        /// <returns>Execution result which contains created row id or error message if failed.</returns>
        public virtual async Task<long> UpdateAsync(T dbo, IDatabaseCommandExecutionLogic databaseCommandExecutionLogic)
        {
            dbo.FillTableInfo();
            _tbl = dbo;
            return await databaseCommandExecutionLogic.UpdateAsync(_updateCommand, CommandParametersHelper.GetDynamicParameters(MapParameters(dbo)));
        }

        /// <summary>
        /// Async command to delete record in database.
        /// </summary>
        /// <param name="dbo">Model to delete.</param>
        /// <returns>Execution result which contains created row id or error message if failed.</returns>
        public virtual async Task<long> DeleteAsync(T dbo, IDatabaseCommandExecutionLogic databaseCommandExecutionLogic)
        {
            dbo.FillTableInfo();
            _tbl = dbo;
            return await databaseCommandExecutionLogic.DeleteAsync(_deleteCommand, CommandParametersHelper.GetDynamicParameters(MapParameters(dbo)));
        }

        /// <summary>
        /// Parameter mapper for table, later is used for dynamic parameter creation.
        /// </summary>
        /// <param name="table">Table to map parameters from/</param>
        /// <returns>Dictionary that contains parameter as key and it's value.</returns>
        private Dictionary<string, object?> MapParameters(T table)
        {
            var result = new Dictionary<string, object?>(table.TableEntriesBase);

            foreach (var item in table.TableEntries)
                result.Add(item.Key, item.Value);

            return result;
        }

        private string GetUpdateStringFormattedParams() => string.Join(", ", _tbl.TableEntries.Select(col => $"{col.Key} = ISNULL(@{col.Key}, [{col.Key}])"));

        private string GetNormalStringFormattedParams(char separator)
        {
            if (!_tbl.TableEntries.Any())
                return string.Empty;

            string parameters = separator == ','
                ? string.Join(", ", _tbl.TableEntries.Select(col => $"[{col.Key}]"))
                : string.Join(", ", _tbl.TableEntries.Select(col => $"@{col.Key}"));
            return parameters + ",";
        }
    }
}
