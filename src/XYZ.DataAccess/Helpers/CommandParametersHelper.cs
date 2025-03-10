using Dapper;

namespace XYZ.DataAccess.Helpers
{
    /// <summary>
    /// Helper for database commands creation.
    /// </summary>
    public static class CommandParametersHelper
    {
        /// <summary>
        /// Creates dynamic parameters for dapper from key (field name) and value (field value).
        /// </summary>
        /// <param name="userParameters">Parameters with key as field name and value as field value.</param>
        /// <returns>DynamicParameters object with mapped values.</returns>
        public static DynamicParameters GetDynamicParameters(Dictionary<string, object?> userParameters)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();

            foreach (KeyValuePair<string, object?> defaultPair in userParameters)
            {
                // Dapper is not overriding datetime values during CUD commands, so if in parameters there is default datetime
                // then it will try to insert it despite of GETDATE() defined for field in sql command...
                if (defaultPair.Value is DateTime dt && dt == DateTime.MinValue)
                    continue;

                dynamicParameters.Add(string.Concat("@", defaultPair.Key), defaultPair.Value);
            }

            return dynamicParameters;
        }
    }
}
