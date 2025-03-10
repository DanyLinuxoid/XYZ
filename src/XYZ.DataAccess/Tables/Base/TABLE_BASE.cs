using System.Reflection;

namespace XYZ.DataAccess.Tables.Base
{
    /// <summary>
    /// Base logic that should be inherited by all tables.
    /// Contains mapping logic, scheme, name, etc.
    /// </summary>
    public abstract class TABLE_BASE : TABLE_RECORDS_BASE
    {
        /// <summary>
        /// Contains children field-value mappings.
        /// </summary>
        protected virtual Dictionary<string, object?> _tableEntries { get; set; } = new Dictionary<string, object?>();

        /// <summary>
        /// Database scheme.
        /// </summary>
        public string Scheme { get; } = "[dbo]";

        /// <summary>
        /// Children table name.
        /// </summary>
        public string TableName { get; protected set; }

        /// <summary>
        /// Public read access for children table entries.
        /// </summary>
        public Dictionary<string, object?> TableEntries => _tableEntries;

        /// <summary>
        /// Auto fills children parameters in field-value format and sets table name.
        /// Is called by query and CUD logics during requests.
        /// </summary>
        public void FillTableInfo()
        {
            var type = GetType();
            TableName = $"[{type.Name}]"; // Setting name
            FillParameters(type); // Filling parameters info (Parent + Children) for CRUD operations
        }

        /// <summary>
        /// Auto parameter fill actual logic.
        /// </summary>
        /// <param name="tblType">Children table type.</param>
        private void FillParameters(Type tblType)
        {
            var properties = tblType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var prop in properties)
            {
                if (!prop.CanRead || prop.DeclaringType == typeof(TABLE_BASE) || prop.DeclaringType == typeof(TABLE_RECORDS_BASE))
                    continue;

                _tableEntries[prop.Name] = prop.GetValue(this);
            }
        }
    }
}
