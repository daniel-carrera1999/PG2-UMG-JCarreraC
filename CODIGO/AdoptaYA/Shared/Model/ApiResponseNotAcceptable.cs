namespace AdoptaYA.Shared.Model;
public class ApiResponseNotAcceptable
{
    public string Type { get; set; }
    public string Title { get; set; }
    public int Status { get; set; }
    public string Detail { get; set; }
    public string Datetime { get; set; }
    public string Instance { get; set; }
    public Dictionary<string, List<string>> Errors { get; set; } = new();
}