using XYZ.DataAccess.Helpers;

namespace XYZ.Tests.DataAccess.Helpers
{
    public class CommandParametersHelperTests
    {
        [Fact]
        public void GetDynamicParameters_ReturnsCorrectParameters()
        {
            // Arrange
            var parameters = new Dictionary<string, object?>
            {
                { "Id", 1 },
                { "Name", "Test" },
                { "IsActive", true }
            };

            // Act
            var result = CommandParametersHelper.GetDynamicParameters(parameters);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Get<int>("@Id"));
            Assert.Equal("Test", result.Get<string>("@Name"));
            Assert.True(result.Get<bool>("@IsActive"));
        }

        [Fact]
        public void GetDynamicParameters_SkipsDefaultDateTime()
        {
            // Arrange
            var parameters = new Dictionary<string, object?>
            {
                { "CreatedAt", DateTime.MinValue },
                { "UpdatedAt", new DateTime(2024, 1, 1) }
            };

            // Act
            var result = CommandParametersHelper.GetDynamicParameters(parameters);

            // Assert
            Assert.DoesNotContain("@CreatedAt", result.ParameterNames);
            Assert.Equal(new DateTime(2024, 1, 1), result.Get<DateTime>("@UpdatedAt"));
        }

        [Fact]
        public void GetDynamicParameters_HandlesNullValues()
        {
            // Arrange
            var parameters = new Dictionary<string, object?>
            {
                { "Description", null }
            };

            // Act
            var result = CommandParametersHelper.GetDynamicParameters(parameters);

            // Assert
            Assert.Null(result.Get<object?>("@Description"));
        }
    }
}
