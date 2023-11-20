using System.ComponentModel.DataAnnotations;

namespace Blog.API.Data.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}