namespace AdvWorksAPI.EntityLayer;

public class AdvWorksAPIDefaults
{
    public AdvWorksAPIDefaults()
    {
        Created = DateTime.Now;
        ProductCategoryID = 1;
        ProductModleID = 2;
    }

    public DateTime Created { get; set; }
    public int ProductCategoryID { get; set; }
    public int ProductModleID { get; set; }
}