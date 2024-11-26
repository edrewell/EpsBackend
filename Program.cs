using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EpsBackend.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<EpsBackendContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EpsBackendContext") ?? throw new InvalidOperationException("Connection string 'EpsBackendContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
