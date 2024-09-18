using System.ComponentModel.DataAnnotations;

namespace MvcPhone.Models;
public class Login {
    public int Id { get; set; } 
    public string Name { get; set; }
    public string Email { get; set;}
    [Display (Name ="Mat Khau")]
    public string Password { get; set;}
    public string? Role { get; set; }
}