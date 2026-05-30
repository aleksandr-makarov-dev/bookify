using System.Text;
using Bookify.Api.Behaviors;
using Bookify.Modules.Booking.Infrastructure;
using Bookify.Modules.Notifications.Infrastructure;
using Bookify.Modules.Scheduling.Infrastructure;
using Bookify.Modules.Users.Infrastructure;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddUsersModule(builder.Configuration);
builder.Services.AddSchedulingModule(builder.Configuration);
builder.Services.AddBookingModule(builder.Configuration);
builder.Services.AddNotificationModule(builder.Configuration);

builder.Services.AddValidatorsFromAssemblies([
    Bookify.Modules.Users.Application.AssemblyReference.Assembly,
    Bookify.Modules.Scheduling.Application.AssemblyReference.Assembly,
    Bookify.Modules.Booking.Application.AssemblyReference.Assembly
]);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies([
        typeof(Bookify.Modules.Users.Application.AssemblyReference).Assembly,
        typeof(Bookify.Modules.Scheduling.Application.AssemblyReference).Assembly,
        typeof(Bookify.Modules.Booking.Application.AssemblyReference).Assembly,
        typeof(Bookify.Modules.Notifications.Application.AssemblyReference).Assembly
    ]);

    cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
});

builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumers(typeof(Program).Assembly);

    configure.SetKebabCaseEndpointNameFormatter();

    configure.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
        {
            host.Username(builder.Configuration["RabbitMq:Username"]);
            host.Password(builder.Configuration["RabbitMq:Password"]);
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = []
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["JsonWebToken:Issuer"],
        ValidAudience = builder.Configuration["JsonWebToken:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JsonWebToken:SecretKey"]!)
        ),

        ClockSkew = TimeSpan.Zero
    };
});


builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

namespace Bookify.Api
{
    public partial class Program
    {
    }
}