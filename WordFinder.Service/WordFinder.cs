using Microsoft.Extensions.Options;
using WordFinder.Service.ConfigValues;
using WordFinder.Service.Interfaces;

namespace WordFinder.Service;

public class WordFinder : IWordFinder
{
    private readonly List<string> _matrix;
    private readonly int _matrix_max_length;
    
    public WordFinder(IEnumerable<string> matrix, IOptions<WordFinderConfig> wordFinderConfig)
    {
        _matrix = matrix.ToList();
        _matrix_max_length = wordFinderConfig.Value?.MaxSize ?? 64;
    }
    
    /// <summary>
    /// Validate if list of values can be converted to a matrix
    /// </summary>
    /// <returns>true if is valid, false if it's not valid matrix</returns>
    public bool IsValidMatrix()
    {
        var matrixCount = _matrix.Count;
        if (matrixCount > _matrix_max_length || _matrix is null ||_matrix.Count == 0)
            return false;
        if (_matrix.Any(x => x.Length != matrixCount) || _matrix.Any(x => x.Length == 0))
            return false;
        
        return true;
    }
    
    /// <summary>
    /// Find all words in matrix
    /// </summary>
    /// <param name="wordstream"></param>
    /// <returns>top 10 most repeated words</returns>
    public IEnumerable<string> Find(IEnumerable<string> wordstream)
    {
        if (!IsValidMatrix())
            return new string[] { };
        
        var wordstreamUnique = wordstream.Distinct().ToList();
        var wordFindCounter = new Dictionary<string, int>();
        //look for unique words
        foreach (var word in wordstreamUnique)
        {
            wordFindCounter[word] = FindWordOccurrences(word);
        }
        
        return wordFindCounter.OrderByDescending(x => x.Value).Select(x => x.Key).Take(10);
    }

    private int FindWordOccurrences(string word)
    {
        var horizontalCount = 0;
        var verticalCount = 0;
        
        //allocate and review over the same matrix in order
        for (int row = 0; row < _matrix.Count; row++)
        {
            for (int col = 0; col < _matrix.Count; col++)
            {
                //if found in horizontal add 1 to the count
                if (ExistsInHorizontal(word, row, col))
                    horizontalCount++;
                //if found in vertical add 1 to the count
                if (ExistsInVertical(word, row, col))
                    verticalCount++;
            }
        }

        return horizontalCount +  verticalCount;
    }

    private bool ExistsInHorizontal(string word, int row, int col)
    {
        //if the column where to start + the lenght of the word pass over the matrix size,
        //then there is not possibility to find the word
        if (col + word.Length > _matrix.Count)
            return false;
        
        for (int i = 0; i < word.Length; i++)
        {
            if (_matrix[row][col + i] != word[i])
                return false;
        }

        return true;
    }
    
    private bool ExistsInVertical(string word, int row, int col)
    {
        if (row + word.Length > _matrix.Count)
            return false;
        
        for (int i = 0; i < word.Length; i++)
        {
            if (_matrix[row+i][col] != word[i])
                return false;
        }
        return true;
    }
}