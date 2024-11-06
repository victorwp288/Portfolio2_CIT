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
using Microsoft.EntityFrameworkCore;
using Npgsql;

// Enable unmapped types globally
NpgsqlConnection.GlobalTypeMapper.MapEnum<UserRole>("user_role");

var builder = WebApplication.CreateBuilder(args);

// Configure Entity Framework Core to use PostgreSQL as the database provider
builder.Services.AddDbContext<ImdbContext>(options =>
    options.UseNpgsql("host=localhost;db=imdb;uid=postgres;pwd=Ferieland128"));

// Register IDataService with its implementation, DataService, using scoped lifetime
builder.Services.AddScoped<IDataService, DataService>();

builder.Services.AddScoped<ITitleService, TitleService>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IRatingService, RatingService>();

builder.Services.AddScoped<IBookmarkService, BookmarkService>();

// Get the connection string from ImdbContext
var connectionString = "host=localhost;db=imdb;uid=postgres;pwd=Ferieland128";

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