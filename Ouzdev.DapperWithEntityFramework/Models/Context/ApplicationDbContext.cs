using Microsoft.EntityFrameworkCore;
using Ouzdev.DapperWithEntityFramework.Models.Entities;
using System.Data;

namespace Ouzdev.DapperWithEntityFramework.Models.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public IDbConnection Connection => Database.GetDbConnection();
    }
}
