using filemanagementapi.Domain.FileModel;
using filemanagementapi.Domain.UserModel;
using Microsoft.EntityFrameworkCore;

namespace filemanagementapi.Context;
public class DatabaseContext:DbContext
{
    public DatabaseContext(DbContextOptions options):base(options)
    {
        
    }
    public DbSet<UserModel> userModels {get;set;}
    public DbSet<FileModel> FileModels {get;set;}
}
