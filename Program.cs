global using App_www_zaliczenie.Models;
global using App_www_zaliczenie.Data;
global using App_www_zaliczenie.Services.GameService;
global using App_www_zaliczenie.Services.RankingService;
global using App_www_zaliczenie.Dtos;
global using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( options => 
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IRankingService, RankingService>();

// ustawienia do polaczenia bazy danych z naszym API
builder.Services.AddDbContext<DataContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

 
builder.Services.AddAuthorization();

builder.Services.AddIdentity<Account, IdentityRole<int>>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<Account>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
