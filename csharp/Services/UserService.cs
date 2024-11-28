using Data;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    // 哈希密码
    private static string HashPassword(string password)
    {
        // BCrypt 会自动生成一个盐值并包含在哈希中
        return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
    }

    public async Task Add(User user)
    {
        user.UserPwd = HashPassword(user.UserPwd);
        await _context.User.AddAsync(user);
        await _context.SaveChangesAsync();
        // if (await _context.SaveChangesAsync() > 0)
        //     await _emailService.Send(_configuration["EmailAddress"]!, $"New todo has been added: {todo.Title}");

    }

    public async ValueTask<User?> Find(int id)
    {
        return await _context.User.FindAsync(id);
    }
    public async ValueTask<User?> FindByName(string name)
    {
        return await _context.User.FirstAsync(e => e.UserName == name);
    }

    public async Task<List<UserDto>> List()
    {
        var list = await _context.User.ToListAsync();
        var users = from b in list
                    select new UserDto()
                    {
                        UserId = b.UserId,
                        UserName = b.UserName,
                        UserAge = b.UserAge,
                        UserAddress = b.UserAddress
                    };
        return users.ToList();
    }

    public async Task Remove(User user)
    {
        _context.User.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task Update(User user)
    {
        _context.User.Update(user);
        await _context.SaveChangesAsync();
    }

}