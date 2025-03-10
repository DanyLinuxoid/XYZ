using XYZ.DataAccess.Tables.Base;

namespace XYZ.DataAccess.Base
{
    public abstract class QueryBase<T> where T : TABLE_BASE, new()
    {
        protected T _tbl { get; }

        public QueryBase()
        {
            _tbl = new T();
            _tbl.FillTableInfo();
        }

        public abstract string SqlQuery { get; }

        protected string TableBasicParams => string.Join(", ", _tbl.TableEntriesBase.Select(col => $"{col.Key} AS {col.Key}"));
        protected string TableSpecificParams => string.Join(", ", _tbl.TableEntries.Select(col => $"{col.Key} AS {col.Key}"));
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
