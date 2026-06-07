namespace MvcHv.Models;

public class SearchApiRequest
{
    public string? Query { get; set; }
    public int TopK { get; set; } = 5;
}
