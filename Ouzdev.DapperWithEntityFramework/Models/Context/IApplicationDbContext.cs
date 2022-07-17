using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Ouzdev.DapperWithEntityFramework.Models.Entities;
using System.Data;

namespace Ouzdev.DapperWithEntityFramework.Models.Context
{
    public interface IApplicationDbContext
    {
        //Database e bağlanmak için kullanılan arayüzdür.
        public IDbConnection Connection { get; }
        // Contexti parametre olarak alarak Transaction işlemlerini yaparız.
        DatabaseFacade Database { get; }
        //DbSet işlemlerini burada yapıyoruz ve sonrasında ApplicationDbContext sınıfına implement ediyoruz.
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        //CancellationToken, async işlemlerde ilgili thread i sonlandıran bir sınıfdır. 
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
