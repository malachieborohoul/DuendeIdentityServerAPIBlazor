using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class ApplicationUser:IdentityUser
{
    public string? Name { get; set; }
    
     
   

}