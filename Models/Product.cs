using System.ComponentModel.DataAnnotations;
namespace MvcPhone.Models;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }
    public string? Genre { get; set; }
    public decimal Price { get; set; }
    public int? Amount { get; set; }
    
    // Khóa ngoại
    
    public int? CategoryId { get; set; }

    // Điều hướng đến Category (bảng cha)
    public Category? Category { get; set; } = null!; // Điều hướng đến Category
    public ICollection<ProductBill> ProductBills { get; set; } = new List<ProductBill>();

}