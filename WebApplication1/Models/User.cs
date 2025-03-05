using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;

// Model are classes that represents data which the app manages
public class User
{
    public long Id { get; set; }
    // nullable reference type in modal. Because Ef core manages entity initializing for us, we can suppress the compiler warning
    // use Required attribute so it does not store null in the database. data field value
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    // This is foreign key relationship to role table that will be created
    [Required]
    [ForeignKey("Role")]
    public long RoleId { get; set; }

//   Here we have role property specifying one Role property for one User - this is a type of navigation property
    // public Role Role {get; set;} = null!;
    public Role? Role {get; set;}
}