using TodoApi.Models;

public interface IAuthService
{
    public Task<string> Login(LoginDto dto);
}
