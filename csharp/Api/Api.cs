using WebMinRouteGroup;

namespace MinAPISeparateFile;

public static class TodoEndpoints
{
    public static void Map(WebApplication app)
    {

        app.MapGet("/", context =>
        {
            context.Response.Redirect("/swagger/index.html");
            return Task.CompletedTask;
        });

        app.MapGroup("/todos/v1/auth")
        .MapAuthApi()
        .WithTags("Auth");

        app.MapGroup("/todos/v1/user")
        .MapUserApi().RequireAuthorization()
        .WithTags("User");
    }
}