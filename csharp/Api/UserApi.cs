using Microsoft.AspNetCore.Http.HttpResults;
using TodoApi.Models;

namespace WebMinRouteGroup;

public static class UserApi
{
    public static RouteGroupBuilder MapUserApi(this RouteGroupBuilder group)
    {
        group.MapGet("/list", List);
        group.MapGet("/{id}", Get);
        group.MapPost("/", Create);
        group.MapPut("/{id}", Update);
        group.MapDelete("/{id}", Delete);

        return group;
    }

    public static async Task<Results<Ok<List<UserDto>>, NotFound>> List(IUserService userService)
    {
        var list = await userService.List();

        if (list != null)
        {
            return TypedResults.Ok(list);
        }

        return TypedResults.NotFound();
    }

    public static async Task<Results<Ok<UserDto>, NotFound>> Get(int id, IUserService userService)
    {
        var user = await userService.Find(id);

        if (user != null)
        {
            var dto = new UserDto()
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserAge = user.UserAge,
                UserAddress = user.UserAddress
            };
            return TypedResults.Ok(dto);
        }

        return TypedResults.NotFound();
    }

    // create 
    public static async Task<Created<User>> Create(User user, IUserService userService)
    {
        var newUser = new User
        {
            UserName = user.UserName,
            UserPwd = user.UserPwd,
            UserAge = user.UserAge,
            UserAddress = user.UserAddress,
        };

        await userService.Add(newUser);

        return TypedResults.Created($"/todos/v1/{newUser.UserId}", newUser);
    }

    // update 
    public static async Task<Results<Created<User>, NotFound>> Update(User user, IUserService userService)
    {
        var existingUser = await userService.Find(user.UserId);

        if (existingUser != null)
        {
            existingUser.UserName = user.UserName;
            existingUser.UserPwd = user.UserPwd;
            existingUser.UserAge = user.UserAge;
            existingUser.UserAddress = user.UserAddress;

            await userService.Update(existingUser);

            return TypedResults.Created($"/todos/v1/{existingUser.UserId}", existingUser);
        }

        return TypedResults.NotFound();
    }

    // delete
    public static async Task<Results<NoContent, NotFound>> Delete(int id, IUserService userService)
    {
        var user = await userService.Find(id);

        if (user != null)
        {
            await userService.Remove(user);
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }
}
