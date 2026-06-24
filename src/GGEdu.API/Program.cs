using FluentValidation.AspNetCore;
using GGEdu.API.Filters;
using GGEdu.API.Middlewares;
using GGEdu.Application.Extensions;
using GGEdu.Application.Logging;
using GGEdu.Application.Services.Users;
using GGEdu.CompositionRoot.DependencyInjection;
using GGEdu.Core.Services.Users;
using GGEdu.Infrastructure.Extensions;
using GGEdu.Infrastructure.Repositories;
using GGEdu.Localization.Extensions;
using Microsoft.OpenApi;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.BuildLocalizationServices(builder.Configuration);
builder.Services.BuidDIServices(new List<Assembly> { typeof(GenericRepository<>).Assembly, typeof(UserService).Assembly });
builder.Services.BuildInfrastructureServices(builder.Configuration);
builder.Services.BuildApplication(builder.Configuration);
builder.Services.BuildLogger(builder.Configuration);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins",
            policyBuilder => policyBuilder.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                //.AllowCredentials()
                .SetIsOriginAllowed(_ => true)
        );
    });
}

builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute()));
builder.Services.AddFluentValidationAutoValidation();


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement((document) => new OpenApiSecurityRequirement()
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAllOrigins");

    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<LoggingMiddeware>();

app.MapControllers();

app.Run();
