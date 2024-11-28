using TodoApi.Models;

public interface IUserService
{
    Task<List<UserDto>> List();

    ValueTask<User?> Find(int id);
    ValueTask<User?> FindByName(string name);

    Task Add(User user);

    Task Update(User user);

    Task Remove(User user);
}
