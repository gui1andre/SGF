using Application.Faturas;
using Application.Faturas.Ports;
using Application.Validators;
using Domain.Faturas.Interfaces;
using FluentValidation;
using Infrastructure.Repositories;
using SGF.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IFaturaManager, FaturaManager>();
builder.Services.AddScoped<IFaturaRepository, FaturaRepository>();

builder.Services.AddValidatorsFromAssembly(typeof(CriarFaturaDTOValidator).Assembly);

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<ExceptionHadleMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
