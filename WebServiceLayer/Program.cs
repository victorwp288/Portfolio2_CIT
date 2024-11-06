using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataAccessLayer.Repositories.Implementations;
using DataAcessLayer;
using DataAcessLayer.Context;
using DataAcessLayer.Repositories.Implementations;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using DataAcessLayer.Entities.Users; // For UserRole enum
using Npgsql.EntityFrameworkCore.PostgreSQL; // Add this

// Enable unmapped types globally
NpgsqlConnection.GlobalTypeMapper.MapEnum<UserRole>("user_role");

var builder = WebApplication.CreateBuilder(args);

// Configure Entity Framework Core to use PostgreSQL as the database provider
builder.Services.AddDbContext<ImdbContext>(options =>
    options.UseNpgsql("host=localhost;db=imdb;uid=postgres;pwd=2409"));

// Register IDataService with its implementation, DataService, using scoped lifetime
builder.Services.AddScoped<IDataService, DataService>();

builder.Services.AddScoped<ITitleService, TitleService>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IRatingService, RatingService>();

// Register the repository
builder.Services.AddScoped<IMovieSearchRepository, MovieSearchRepository>();

// Register the search service
builder.Services.AddScoped<ISearchService, SearchService>();

// Register Mapster to handle object mapping automatically between data models and DTOs
builder.Services.AddMapster();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Console.WriteLine(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters

    {

        ValidateIssuer = false,

        ValidateAudience = false,

        ValidateLifetime = true,

        ValidateIssuerSigningKey = true,

        //ValidIssuer = builder.Configuration["Jwt: Issuer"],

        //ValidAudience = builder.Configuration["Jwt: Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ClockSkew = TimeSpan.Zero,
    };
});


var app = builder.Build();

app.UseAuthentication();

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