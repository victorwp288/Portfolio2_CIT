using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataAccessLayer.Repositories.Implementations;
using DataAcessLayer;
using DataAcessLayer.Context;
using DataAcessLayer.Repositories.Implementations;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using DataAcessLayer.Entities.Users; // For UserRole enum
using Npgsql.EntityFrameworkCore.PostgreSQL; // Add this

// Enable unmapped types globally
NpgsqlConnection.GlobalTypeMapper.MapEnum<UserRole>("user_role");

var builder = WebApplication.CreateBuilder(args);

// Configure Entity Framework Core to use PostgreSQL as the database provider
builder.Services.AddDbContext<ImdbContext>(options =>
    options.UseNpgsql("host=localhost;db=imdb;uid=postgres;pwd=Hejmed12!"));

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