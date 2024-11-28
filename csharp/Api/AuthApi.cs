using Microsoft.AspNetCore.Http.HttpResults;
using TodoApi.Models;

namespace WebMinRouteGroup;

public static class AuthApi
{
    public static RouteGroupBuilder MapAuthApi(this RouteGroupBuilder group)
    {
        group.MapPost("/login", Login);
        return group;
    }

    public static async Task<Results<Ok<string>, UnauthorizedHttpResult>> Login(LoginDto dto, IAuthService authService)
    {
        var token = await authService.Login(dto);

        if (token != "")
        {
            return TypedResults.Ok(token);
        }

        return TypedResults.Unauthorized();
    }
}
