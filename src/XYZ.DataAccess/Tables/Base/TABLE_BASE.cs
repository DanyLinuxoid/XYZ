using System.Reflection;

namespace XYZ.DataAccess.Tables.Base
{
    public abstract class TABLE_BASE : TABLE_RECORDS_BASE
    {
        protected virtual Dictionary<string, object?> _tableEntries { get; set; } = new Dictionary<string, object?>();

        public string Scheme { get; } = "[dbo]";
        public string TableName { get; protected set; }
        public Dictionary<string, object?> TableEntries => _tableEntries;

        public void FillTableInfo()
        {
            var type = GetType();
            TableName = $"[{type.Name}]"; // Setting name
            FillParameters(type); // Filling parameters info (Parent + Children) for CRUD operations
        }

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
