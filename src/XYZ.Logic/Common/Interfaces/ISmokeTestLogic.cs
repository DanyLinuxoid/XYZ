namespace XYZ.Logic.Common.Interfaces
{
    /// <summary>
    /// System smoke test logic.
    /// </summary>
    public interface ISmokeTestLogic
    {
        /// <summary>
        /// Main method to determine if system is functional.
        /// </summary>
        /// <returns>Ok if system is functional, false if not.</returns>
        Task<bool> IsSystemFunctional();
    }
}