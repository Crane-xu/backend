using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models;

[Microsoft.EntityFrameworkCore.Index(nameof(UserId), nameof(UserName), IsUnique = true)]
public class User
{
    [Key]   //数据库主键
    public int UserId { get; set; }
    [Column(TypeName = "nvarchar(100)")]
    public required string UserName { get; set; }
    [Column(TypeName = "nvarchar(100)")]
    public required string UserPwd { get; set; }
    public int UserAge { get; set; }
    [Column(TypeName = "nvarchar(200)")]
    public string? UserAddress { get; set; }

}
public class UserDto
{
    public int UserId { get; set; }
    public required string UserName { get; set; }
    public int UserAge { get; set; }
    public string? UserAddress { get; set; }

}