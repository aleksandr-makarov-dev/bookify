using Bookify.Application;
using Bookify.Infrastructure;
using Bookify.Modules.Booking.Infrastructure;
using FluentValidation;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddBookingModule(builder.Configuration);

builder.Services.AddInfrastructureLayer(builder.Configuration, [typeof(Program).Assembly]);
builder.Services.AddApplicationLayer([
    typeof(Bookify.Modules.Booking.Application.AssemblyReference).Assembly,
]);

builder.Services.AddValidatorsFromAssemblies([
    Bookify.Modules.Booking.Application.AssemblyReference.Assembly
]);

builder.Services.AddHttpContextAccessor();
builder.Services.AddProblemDetails();

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