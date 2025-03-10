using XYZ.DataAccess.Tables.USER_TABLE;
using XYZ.Logic.Features.User.Mappers;
using XYZ.Models.Features.User.DataTransfer;

namespace XYZ.Tests.LogicTests.Features.User
{
    public class UserInfoShortMapperTests
    {
        [Fact]
        public void ToDto_ReturnsCorrectUserInfoShort()
        {
            // Arrange
            var user = new USER
            {
                ID = 12345,
                DB_RECORD_CREATION_TIME = new DateTime(2021, 1, 1)
            };

            var expectedDto = new UserInfoShort
            {
                Id = user.ID,
                AccountCreationDateTime = user.DB_RECORD_CREATION_TIME
            };

            // Act
            var result = user.ToDto();

            // Assert
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.AccountCreationDateTime, result.AccountCreationDateTime);
        }
    }
}
