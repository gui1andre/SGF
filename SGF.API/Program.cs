using Application.Faturas;
using Application.Faturas.Ports;
using Application.Validators;
using Domain.Faturas.Interfaces;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SGF.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(connectionString));


builder.Services.AddScoped<IFaturaManager, FaturaManager>();
builder.Services.AddScoped<IFaturaRepository, FaturaRepository>();

builder.Services.AddValidatorsFromAssembly(typeof(CriarFaturaDTOValidator).Assembly);

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<ExceptionHadleMiddleware>();

app.MapOpenApi();
app.MapScalarApiReference();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDBContext>();
    db.Database.Migrate();
}

app.Run();
