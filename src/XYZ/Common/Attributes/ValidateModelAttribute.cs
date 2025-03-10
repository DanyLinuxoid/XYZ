namespace XYZ.Web.Common.Attributes
{
    /// <summary>
    /// Attribute which signals that action/controller should be validated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class ValidateModelAttribute : Attribute
    {
    }
}
