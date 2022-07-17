using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Ouzdev.DapperWithEntityFramework.Interfaces;
using Ouzdev.DapperWithEntityFramework.Models.Context;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var dbType = builder.Configuration.GetConnectionString("DbType");
if (dbType == "SQLServer")
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options
    .UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"))
    );
}
else if (dbType == "PostgreSQL")
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
      options
      .UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL"))
      );
}

builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
builder.Services.AddScoped<IApplicationWriteDbConnection, ApplicationWriteDbConnection>();
builder.Services.AddScoped<IApplicationReadDbConnection, ApplicationReadDbConnection>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();
app.Run();
