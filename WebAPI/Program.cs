using Application.Use_Cases.Role.DTOs;
using FluentValidation;
using WebAPI;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddDatabase(configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddMemoryCache();
builder.Services.AddApplicationServices();
builder.Services.AddRepositories();
builder.Services.AddAutoMapperProfiles();
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
