namespace AdvWorksAPI.BaseClasses;

public class SearchBase
{
    public SearchBase()
    {
        OrderBy = string.Empty;
    }

    public SearchBase(string orderBy)
    {
        OrderBy = orderBy;
    }

    public string OrderBy { get; set; }
}