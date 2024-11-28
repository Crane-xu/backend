using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public required DbSet<User> User { get; set; }  // 替换为你的模型类

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // 在这里添加你的模型配置代码（如果有）
        }
    }
}