using System;
using System.ComponentModel.DataAnnotations;
namespace MvcPhone.Models;
public class ProductBill
{
    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int BillId { get; set; }
    public Bill Bill { get; set; }
}
