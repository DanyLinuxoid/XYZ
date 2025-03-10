namespace XYZ.Models.Features.User.DataTransfer
{
    /// <summary>
    /// User main information.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Main identifier.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Date when account was created.
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
