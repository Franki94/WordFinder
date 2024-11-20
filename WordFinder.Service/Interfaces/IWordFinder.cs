namespace WordFinder.Service.Interfaces;

public interface IWordFinder
{
    bool IsValidMatrix();
    IEnumerable<string> Find(IEnumerable<string> wordstream);
}