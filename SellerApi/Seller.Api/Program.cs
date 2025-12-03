using Microsoft.EntityFrameworkCore;
using Seller.Infrastructure.Data;
using Seller.Infrastructure.Repositories;
using Seller.Core.Interfaces;
using Seller.Api.Services;
 
var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
// Use SQLite instead of SQL Server
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlite("Data Source=seller.db"));
 
builder.Services.AddScoped<ISellerRepository, SellerRepository>();
builder.Services.AddScoped< ISellerService, SellerService>();
 
var app = builder.Build();
 
app.UseSwagger();
app.UseSwaggerUI();
 
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
 
app.Run();
 
 
 