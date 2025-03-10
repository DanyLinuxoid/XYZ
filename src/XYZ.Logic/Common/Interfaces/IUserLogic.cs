using XYZ.Models.Features.User.DataTransfer;

namespace XYZ.Logic.Common.Interfaces
{
    public interface IUserLogic
    {
        Task<long> CreateUser();
        Task<UserInfoShort?> GetUserInfoShort(long id);
    }
}