namespace WordFinderApi.Requests;

public class WorkFinderRequest
{
    public IEnumerable<string> Matrix { get; set; }
    public IEnumerable<string> FindWords { get; set; }
}