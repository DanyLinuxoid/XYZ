namespace XYZ.Models.Common.ExceptionHandling
{
    public class ProblemDetailed
    {
        public string Title { get; }

        public string Details { get; }

        public bool IsException { get; }

        public ProblemDetailed(string title, string details, bool isException = false)
        {
            Title = title;
            Details = details;
        }
    }
}
