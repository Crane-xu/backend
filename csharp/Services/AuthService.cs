using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoApi.Models;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;

    public AuthService(IConfiguration configuration, IUserService userService)
    {
        _configuration = configuration;
        _userService = userService;
    }

    // 验证密码
    private static bool VerifyPassword(string inputPassword, string hashedPassword)
    {
        // BCrypt 会自动从哈希中提取盐值并进行验证
        return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
    }

    public async Task<string> Login(LoginDto dto)
    {
        var u = await _userService.FindByName(dto.Username);
        if (u == null)
        {
            return "";
        }
        // 假设这里有一个验证逻辑，验证用户名和密码
        if (VerifyPassword(dto.Password, u.UserPwd))
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Name, dto.Username)
                ]),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        else
        {
            return "";
        }
    }
}

