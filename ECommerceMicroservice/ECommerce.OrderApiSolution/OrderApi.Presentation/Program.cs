using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrderApi.Infrastructure.Data;
using OrderApi.Infrastructure.Data.DependencyInjection;
using OrderApi.Application.DependencyInjection;
using Serilog;
using MassTransit;
using OrderApi.Presentation.Consumer;
using ProductApi.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Сохраняем лог в файл при старте приложения
Log.Information("Starting application");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddApplicationService(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UserInfrastructurePolicy();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
