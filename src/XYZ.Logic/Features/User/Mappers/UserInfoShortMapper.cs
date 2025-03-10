using XYZ.DataAccess.Tables.USER_TABLE;
using XYZ.Models.Features.User.DataTransfer;

namespace XYZ.Logic.Features.User.Mappers
{
    public static class UserInfoShortMapper
    {
        public static UserInfoShort ToDto(this USER user)
        {
            return new UserInfoShort()
            {
                Id = user.ID,
                AccountCreationDateTime = user.DB_RECORD_CREATION_TIME,
            };
        }
    }
}
