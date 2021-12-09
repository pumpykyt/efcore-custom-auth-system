using CustomAuthSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomAuthSystem.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
}