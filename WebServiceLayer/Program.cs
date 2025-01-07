using BuisnessLayer.Interfaces;
using BuisnessLayer.Services;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataAccessLayer.Repositories.Implementations;
using DataAcessLayer;
using DataAcessLayer.Context;
using DataAcessLayer.Repositories.Implementations;
using DataAcessLayer.Repositories.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Text;



// Enable unmapped types globally
NpgsqlConnection.GlobalTypeMapper.MapEnum<UserRole>("user_role");

var builder = WebApplication.CreateBuilder(args);

// Configure Entity Framework Core to use PostgreSQL as the database provider
builder.Services.AddDbContext<ImdbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ImdbDatabase")));


// Register IDataService with its implementation, DataService, using scoped lifetime
builder.Services.AddScoped<IDataService, DataService>();

builder.Services.AddScoped<ITitleService, TitleService>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IRatingService, RatingService>();

builder.Services.AddScoped<IBookmarkService, BookmarkService>();

builder.Services.AddScoped<IHasherService, HasherService>();

// Get the connection string from ImdbContext
var connectionString = builder.Configuration.GetConnectionString("ImdbDatabase");

// Register repositories
builder.Services.AddScoped<IMovieSearchRepository, MovieSearchRepository>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<ISearchHistoryRepository>(provider =>
    new SearchHistoryRepository(connectionString));

// Register Mapster to handle object mapping automatically between data models and DTOs
builder.Services.AddMapster();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Console.WriteLine(builder.Configuration["Jwt:Key"]);

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });

// Add this before builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();


        });
});

var app = builder.Build();

// Add this before app.UseAuthorization();
app.UseCors("AllowAll");

// ... other middleware configurations ...

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();