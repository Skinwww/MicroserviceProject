using ECommercelib.SharedLibrary.DTOs;
using EmailWebhook.Consumer;
using EmailWebhook.Services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IEmailService, EmailService>();
builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.AddConsumer<WebhookConsumer>();
    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"]!);
            h.Password(builder.Configuration["MessageBroker:Password"]!);
        });

        configurator.ReceiveEndpoint("email-webhook-queue", e =>
        {
            e.ConfigureConsumer<WebhookConsumer>(context);
        });
    });
});

var app = builder.Build();

app.MapPost("/email-webhook", ([FromBody] EmailDTO email, IEmailService emailService) =>
{
    string result = emailService.SendEmail(email);
    return Task.FromResult(result);
});

app.Run();
