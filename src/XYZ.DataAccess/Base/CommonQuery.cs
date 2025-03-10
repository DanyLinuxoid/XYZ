using XYZ.DataAccess.Tables.Base;

namespace XYZ.DataAccess.Base
{
    /// <summary>
    /// Query class which contains actual model with parameters.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class QueryBase<T> where T : TABLE_BASE, new()
    {
        /// <summary>
        /// Model that is used during query execution as field pointer, doesn't store data, only field names/methods for reference.
        /// </summary>
        protected T _tbl { get; }

        /// <summary>
        /// Query class which contains actual model with parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public QueryBase()
        {
            _tbl = new T();
            _tbl.FillTableInfo();
        }

        /// <summary>
        /// Actual executable sql query.
        /// </summary>
        public abstract string SqlQuery { get; }

        /// <summary>
        /// Basic parameters formatting.
        /// </summary>
        protected string TableBasicParams => string.Join(", ", _tbl.TableEntriesBase.Select(col => $"{col.Key} AS {col.Key}"));

        /// <summary>
        /// Specific parameters formatting.
        /// </summary>
        protected string TableSpecificParams => string.Join(", ", _tbl.TableEntries.Select(col => $"{col.Key} AS {col.Key}"));

        /// <summary>
        /// Basic parameters + Specific parameters combined.
        /// </summary>
        protected string TableAllParams 
        { 
            get
            {
                var basicParams = TableBasicParams;
                var specificParams = TableSpecificParams;
                return specificParams.Any() ? basicParams + "," + specificParams : basicParams;
            }
        }
    }
}
