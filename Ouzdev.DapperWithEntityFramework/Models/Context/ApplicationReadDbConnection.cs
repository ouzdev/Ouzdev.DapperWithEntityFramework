using Dapper;
using Microsoft.EntityFrameworkCore;
using Ouzdev.DapperWithEntityFramework.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace Ouzdev.DapperWithEntityFramework.Models.Context
{
    public class ApplicationReadDbConnection : IApplicationReadDbConnection, IDisposable
    {
        private readonly IDbConnection connection;
        public ApplicationReadDbConnection(IConfiguration configuration)
        {
            var sqlConnectionString = configuration.GetConnectionString("DbType");
            if (sqlConnectionString == "SQLServer")
            {
                connection = new SqlConnection(configuration.GetConnectionString("SQLServer"));

            }
            else if (sqlConnectionString == "PostgreSQL")
            {
                connection = new SqlConnection(configuration.GetConnectionString("PostgreSQL"));

            }
        }
        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return (await connection.QueryAsync<T>(sql, param, transaction)).AsList();
        }
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
        }
        public async Task<T> QuerySingleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await connection.QuerySingleAsync<T>(sql, param, transaction);
        }
        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
