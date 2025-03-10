using XYZ.DataAccess.Tables.USER_TABLE;
using XYZ.Models.Features.User.DataTransfer;

namespace XYZ.Logic.Features.User.Mappers
{
    /// <summary>
    /// Dto<->Dbo relationship mapping.
    /// </summary>
    public static class UserInfoMapper
    {
        /// <summary>
        /// Maps Dbo object to Dto object.
        /// </summary>
        /// <param name="user">User dbo info.</param>
        /// <returns>User dto object.</returns>
        public static UserDto ToDto(this USER user)
        {
            return new UserDto()
            {
                Id = user.ID,
                CreationTime = user.DB_RECORD_CREATION_TIME,
            };
        }
    }
}
