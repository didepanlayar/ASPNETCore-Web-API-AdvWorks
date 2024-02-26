using AdvWorksAPI.BaseClasses;

namespace AdvWorksAPI.SearchClasses;

public class ProductSearch : SearchBase
{
    public ProductSearch()
    {
        OrderBy = "Name";
        Name = string.Empty;
    }

    public string Name { get; set; }
    public decimal? ListPrice { get; set; }
}
