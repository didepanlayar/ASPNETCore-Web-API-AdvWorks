using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvWorksAPI.EntityLayer;

[Table("Product", Schema = "SalesLT")]
public partial class Product
{
    public Product()
    {
        Name = string.Empty;
        ProductNumber = string.Empty;
        Color = string.Empty;
        Size = string.Empty;
    }

    [Required]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductID { get; set; }
    [Required(ErrorMessage = "The Product Name is required")]
    [StringLength(50, MinimumLength = 4)]
    public string Name { get; set; }
    [Required]
    [StringLength(25, MinimumLength = 3)]
    public string ProductNumber { get; set; }
    public string? Color { get; set; }
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0.01, 9999)]
    public decimal StandardCost { get; set; }
    [Required]
    [Range(0.01, 9999)]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal ListPrice { get; set; }
    [StringLength(5)]
    public string? Size { get; set; }
    [Column(TypeName = "decimal(8, 2)")]
    [Range(0.05, 2000)]
    public decimal? Weight { get; set; }
    public int ProductCategoryID { get; set; }
    public int ProductModelID { get; set; }
    [Required]
    [Range(typeof(DateTime), "2020-01-01", "2030-12-31")]
    public DateTime SellStartDate { get; set; }
    public DateTime? SellEndDate { get; set; }
    public DateTime? DiscontinuedDate { get; set; }
    [Required]
    [Column("rowguid")]
    public Guid RowGuid { get; set; }
    [Required]
    public DateTime ModifiedDate { get; set; }
}
