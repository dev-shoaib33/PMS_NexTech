namespace PMS.API.Models;

using System.ComponentModel.DataAnnotations;

public class ProductVM
{
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Product Name is required")]
    public string ProductName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Product Code is required")]
    public string ProductCode { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
    public float Price { get; set; }

    [Required(ErrorMessage = "Category ID is required")]
    public int CategoryLkpId { get; set; }
    public string? CategoryLkpName { get; set; }

    [RegularExpression(@"^.*\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$", ErrorMessage = "Invalid image file format")]
    public string ImageName { get; set; } = string.Empty;

    public string Uom { get; set; } = string.Empty;
    public string? CreatedBy { get; set; } = null!;
}

