using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApplication1.Models;

public class Role
{
    public long Id { get; set; }

    [Required]
    public string? Name { get; set; }

//  This is a collection of User objects, showing one to many relationship(a role can have zero or more user - navigation property)
//  like a role can have multiple users fxp roles named Admin can have a number of users
    [JsonIgnore] // Prevents infinite loops when serializing - Don't need Role.Users
    public ICollection<User> Users {get; set;} = null!;

}