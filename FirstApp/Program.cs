using FirstApp.DBConnection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;

services.Configure<DBConnectionSetting>(
    configuration.GetSection(nameof(DBConnectionSetting)));

services.AddSingleton<IDatabseSetting>(sp =>
sp.GetRequiredService<IOptions<DBConnectionSetting>>().Value);

services.AddSingleton<DBService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
