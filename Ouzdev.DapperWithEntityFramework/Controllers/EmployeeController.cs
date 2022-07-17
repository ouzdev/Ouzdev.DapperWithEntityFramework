using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ouzdev.DapperWithEntityFramework.Interfaces;
using Ouzdev.DapperWithEntityFramework.Models.Context;
using Ouzdev.DapperWithEntityFramework.Models.Dtos;
using Ouzdev.DapperWithEntityFramework.Models.Entities;
using System.Data.Common;

namespace Ouzdev.DapperWithEntityFramework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class EmployeeController : ControllerBase
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IApplicationReadDbConnection _readDbConnection;
        private readonly IApplicationWriteDbConnection _writeDbConnection;

        public EmployeeController(IApplicationDbContext dbContext, IApplicationReadDbConnection readDbConnection, IApplicationWriteDbConnection writeDbConnection)
        {
            _dbContext = dbContext;
            _readDbConnection = readDbConnection;
            _writeDbConnection = writeDbConnection;
        }
        // GET: api/<EmployeeController>
        [HttpGet]
        public async Task<ActionResult> Get()
        {

            var query = $"SELECT * FROM Employees";
            //var result = Task.Run(async () => await _readDbConnection.QueryAsync<Employee>(query)).Result;
            var response = await _readDbConnection.QueryAsync<Employee>(query);
            return Ok(response);
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id, bool eagerLoading)
        {
            if (eagerLoading)
            {
                var responseEfCore = await _dbContext.Employees.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id  == id);
                return Ok(responseEfCore);
            }
            var query = $"SELECT * FROM Employees Where Id = {id}";
            IEnumerable<Employee> response = await _readDbConnection.QueryAsync<Employee>(query);
            return Ok(response);
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public async Task<ActionResult> AddNewEmployeeWithDepartment([FromBody] EmployeeDto employeeDto)
        {
            _dbContext.Connection.Open();

            using (var transaction = _dbContext.Connection.BeginTransaction())
            {
                try
                {
                    _dbContext.Database.UseTransaction(transaction as DbTransaction);
                    //Check if Department Exists (By Name)
                    bool DepartmentExists = await _dbContext.Departments.AnyAsync(a => a.Name == employeeDto.Department.Name);
                    if (DepartmentExists)
                    {
                        throw new Exception("Department Already Exists");
                    }
                    //Add Department
                    var addDepartmentQuery = $"INSERT INTO Departments(Name,Description) VALUES('{employeeDto.Department.Name}','{employeeDto.Department.Description}');SELECT CAST(SCOPE_IDENTITY() as int)";
                    var departmentId = await _writeDbConnection.QuerySingleAsync<int>(addDepartmentQuery, transaction: transaction);
                    //Check if Department Id is not Zero.
                    if (departmentId == 0)
                    {
                        throw new Exception("Department Id");
                    }
                    //Add Employee
                    var employee = new Employee
                    {
                        DepartmentId = departmentId,
                        Name = employeeDto.Name,
                        Email = employeeDto.Email
                    };
                    await _dbContext.Employees.AddAsync(employee);
                    await _dbContext.SaveChangesAsync(default);
                    //Commmit
                    transaction.Commit();
                    //Return EmployeeId
                    return Ok(employee.Id);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    _dbContext.Connection.Close();
                }
            }
        }


    }
}
