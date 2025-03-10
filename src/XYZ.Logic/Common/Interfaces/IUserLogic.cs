using XYZ.Models.Features.User.DataTransfer;

namespace XYZ.Logic.Common.Interfaces
{
    /// <summary>
    /// Main user management logic.
    /// </summary>
    public interface IUserLogic
    {
        /// <summary>
        /// Simple user creation.
        /// </summary>
        /// <returns>User created main identifier.</returns>
        Task<long> CreateUser();

        /// <summary>
        /// Gets user info by main identifier.
        /// </summary>
        /// <param name="id">Main identifier.</param>
        /// <returns>User if found, null if not found.</returns>
        Task<UserDto?> GetUserInfo(long id);
    }
}