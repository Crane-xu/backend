// ServiceCollectionExtensions.cs
using System.Text;
using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

public static class Application
{
    // Services Add
    public static IServiceCollection AddAppServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        // services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        // 添加身份验证服务
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });
        services.AddAuthorization();

        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
        });

        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IUserService, UserService>();

        // 注册操作主库的数据上下文
        var connectionString = configuration.GetConnectionString("MySqlDataBase");
        services.AddDbContext<ApplicationDbContext>(
            options =>
            {
                options
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
            });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            //Bearer 的scheme定义
            var securityScheme = new OpenApiSecurityScheme()
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                //参数添加在头部
                In = ParameterLocation.Header,
                //使用Authorize头部
                Type = SecuritySchemeType.Http,
                //内容为以 bearer开头
                Scheme = "bearer",
                BearerFormat = "JWT"
            };

            //把所有方法配置为增加bearer头部信息
            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "bearerAuth"
                        }
                    },
                    new string[] {}
                }
            };

            //注册到swagger中
            c.AddSecurityDefinition("bearerAuth", securityScheme);
            c.AddSecurityRequirement(securityRequirement);
        });

        return services;
    }

    // app Use 
    public static WebApplication UseMiddleware(this WebApplication app)
    {

        // using var scope = app.Services.CreateScope();
        // var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
        // db?.Database.MigrateAsync();

        // 添加中间件配置
        app.Use(async (context, next) =>
        {
            // 中间件逻辑
            await next.Invoke();
        });

        // 你可以继续添加更多的中间件
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        // app.MapControllers();
        return app; // 返回WebApplication实例以便链式调用
    }
}