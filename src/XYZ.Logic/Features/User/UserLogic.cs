using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.USER_TABLE;
using XYZ.DataAccess.Tables.USER_TBL.Queries;
using XYZ.Logic.Common.Interfaces;
using XYZ.Logic.Features.User.Mappers;
using XYZ.Models.Features.User.DataTransfer;

namespace XYZ.Logic.Features.User
{
    /// <summary>
    /// Main user management logic.
    /// </summary>
    public class UserLogic : IUserLogic
    {
        /// <summary>
        /// Database access.
        /// </summary>
        public IDatabaseLogic _databaseLogic;

        /// <summary>
        /// User management constructor.
        /// </summary>
        /// <param name="databaseLogic">Database access.</param>
        public UserLogic(IDatabaseLogic databaseLogic)
        {
            _databaseLogic = databaseLogic;
        }

        /// <summary>
        /// Simple user creation.
        /// </summary>
        /// <returns>User created main identifier.</returns>
        public async Task<long> CreateUser()
        {
            var newUser = new USER();
            return await _databaseLogic.CommandAsync(new USER_CUD(), CommandTypes.Create, newUser);
        }

        /// <summary>
        /// Gets user info by main identifier.
        /// </summary>
        /// <param name="id">Main identifier.</param>
        /// <returns>User if found, null if not found.</returns>
        public async Task<UserDto?> GetUserInfo(long id)
        {
            var user = await _databaseLogic.QueryAsync(new UserGetByIdQuery(id));
            if (user == null)
                return null;

            return user.ToDto();
        }
    }
}
