using System.ComponentModel.DataAnnotations;

namespace OptiOverflow.Core.Dtos;

public class LoginDto
{
    [Required(ErrorMessage = "UserName is required")]
    public string UserName { get; set; } = null!;
    
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;
}