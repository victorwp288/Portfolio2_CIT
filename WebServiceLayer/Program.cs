using DataAcessLayer;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Mapster;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure Entity Framework Core to use PostgreSQL as the database provider
builder.Services.AddDbContext<ImdbContext>(options =>
    options.UseNpgsql("host=localhost;db=imdb;uid=postgres;pwd=Ferieland128"));

// Register IDataService with its implementation, DataService, using scoped lifetime
builder.Services.AddScoped<IDataService, DataService>();

builder.Services.AddScoped<ITitleService, TitleService>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IRatingService, RatingService>();

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
