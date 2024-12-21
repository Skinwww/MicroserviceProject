using ECommercelib.SharedLibrary.DTOs;
using EmailWebhook.Consumer;
using EmailWebhook.Dependency_Injection;
using EmailWebhook.Services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureService(builder.Configuration);

var app = builder.Build();

app.MapPost("/email-webhook", ([FromBody] EmailDTO email, IEmailService emailService) =>
{
    string result = emailService.SendEmail(email);
    return Task.FromResult(result);
});
app.Run();
