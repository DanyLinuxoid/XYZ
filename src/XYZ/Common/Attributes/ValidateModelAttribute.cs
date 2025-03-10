namespace XYZ.Web.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class ValidateModelAttribute : Attribute
    {
    }
}
