using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcPhone.Models;

public class ProductEditView
{
    public Product? Product { get; set; }
    [Display(Name = "Category")]
    public List<Category>? Genres { get; set; }
}