using Microsoft.EntityFrameworkCore;
using UNN1N9_SOF_2022231_BACKEND.Data;
using Npgsql;
using UNN1N9_SOF_2022231_BACKEND.Seeds;
using UNN1N9_SOF_2022231_BACKEND.Interfaces;
using UNN1N9_SOF_2022231_BACKEND.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UNN1N9_SOF_2022231_BACKEND.Logic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddDbContext<DataContext>(options =>
{
    options
        .UseNpgsql(connectionString);
});

builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddTransient<IMusicLogic, MusicLogic>();
builder.Services.AddTransient<DataContext, DataContext>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

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

app.UseRouting();

app.UseCors(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:4200")
    );

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Seed();
app.Run();
