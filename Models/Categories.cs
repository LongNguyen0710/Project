using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcPhone.Models;

public class Category
{
    public int Id { get; set; }
    [Display (Name ="Category")]
    public string? Name { get; set; }
    public Product? Product { get; set; } = null; // Điều hướng đến Product

}