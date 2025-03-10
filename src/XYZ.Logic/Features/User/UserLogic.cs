using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.USER_TABLE;
using XYZ.DataAccess.Tables.USER_TBL.Queries;
using XYZ.Logic.Common.Interfaces;
using XYZ.Logic.Features.User.Mappers;
using XYZ.Models.Features.User.DataTransfer;

namespace XYZ.Logic.Features.User
{
    public class UserLogic : IUserLogic
    {
        public IDatabaseLogic _databaseLogic;

        public UserLogic(IDatabaseLogic databaseLogic)
        {
            _databaseLogic = databaseLogic;
        }

        public async Task<long> CreateUser()
        {
            var newUser = new USER();
            return await _databaseLogic.CommandAsync(new USER_CUD(), CommandTypes.Create, newUser);
        }

        public async Task<UserInfoShort?> GetUserInfoShort(long id)
        {
            var user = await _databaseLogic.QueryAsync(new UserGetByIdQuery(id));
            if (user == null)
                return null;

            return user.ToDto();
        }
    }
}
