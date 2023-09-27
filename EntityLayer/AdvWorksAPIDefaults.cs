namespace AdvWorksAPI.EntityLayer;

public class AdvWorksAPIDefaults
{
    public AdvWorksAPIDefaults()
    {
        Created = DateTime.Now;
        InfoMessageDefault = string.Empty;
        ProductCategoryID = 1;
        ProductModelID = 2;
    }

    public DateTime Created { get; set; }
    public string InfoMessageDefault { get; set; }
    public int ProductCategoryID { get; set; }
    public int ProductModelID { get; set; }
}