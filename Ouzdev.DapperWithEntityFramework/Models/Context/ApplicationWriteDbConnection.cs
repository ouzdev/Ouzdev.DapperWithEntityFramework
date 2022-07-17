using Dapper;
using Microsoft.EntityFrameworkCore;
using Ouzdev.DapperWithEntityFramework.Interfaces;
using System.Data;

namespace Ouzdev.DapperWithEntityFramework.Models.Context
{
    public class ApplicationWriteDbConnection : IApplicationWriteDbConnection
    {
        private readonly IApplicationDbContext context;
        public ApplicationWriteDbConnection(IApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await context.Connection.ExecuteAsync(sql, param, transaction);
        }
        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return (await context.Connection.QueryAsync<T>(sql, param, transaction)).AsList();
        }
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await context.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
        }
        public async Task<T> QuerySingleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await context.Connection.QuerySingleAsync<T>(sql, param, transaction);
        }
    }
}
