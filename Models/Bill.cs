using System;
using System.ComponentModel.DataAnnotations;
namespace MvcPhone.Models;

public class Bill {
    public int Id { get; set; }
    public int? ProductId { get; set; }
    public Product? Product { get; set; }
    public DateTime? DateTime { get; set; }
    public int? Amount { get; set; }
    [Display (Name ="Total Bill")]
    public decimal? Price { get; set; }
    public ICollection<ProductBill> ProductBills { get; set; } = new List<ProductBill>();

}