namespace XYZ.Models.Common.ExceptionHandling
{
    /// <summary>
    /// Problem details that is filled in case of errors on server side or during 3-rd party request.
    /// </summary>
    public class ProblemDetailed
    {
        /// <summary>
        /// Problem title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Problem description.
        /// </summary>
        public string Details { get; }

        /// <summary>
        /// If problem is related to an exception.
        /// </summary>
        public bool IsException { get; }

        /// <summary>
        /// Creation constructor.
        /// </summary>
        /// <param name="title">Problem title.</param>
        /// <param name="details">Problem description.</param>
        /// <param name="isException">If problem is related to an exception.</param>
        public ProblemDetailed(string title, string details, bool isException = false)
        {
            Title = title;
            Details = details;
            IsException = isException;
        }
    }
}
