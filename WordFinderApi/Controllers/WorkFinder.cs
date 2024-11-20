using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WordFinder.Service.ConfigValues;
using WordFinderApi.Requests;

namespace WordFinderApi.Controllers;

public class WorkFinder : Controller
{
    private readonly IOptions<WordFinderConfig> _wordFinderConfig;
    public WorkFinder(IOptions<WordFinderConfig> wordFinderConfig)
    {
        _wordFinderConfig = wordFinderConfig;
    }
    
    [HttpPost("find")]
    public IActionResult WordFinder([FromBody]WorkFinderRequest request)
    {
        if (request == null)
            return BadRequest();
        
        var wordFinder = new WordFinder.Service.WordFinder(request.Matrix, _wordFinderConfig);
        if (!wordFinder.IsValidMatrix())
            return BadRequest("Matrix is not Valid");
        
        return Ok(wordFinder.Find(request.FindWords));
    }
}