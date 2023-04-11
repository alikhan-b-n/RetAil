using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using RetAil.Api.Controllers;
using RetAil.Bll.Services;
using RetAil.Bll.Services.Abstract;
using RetAil.Contracts.Options;
using RetAil.Dal;
using RetAil.Dal.Providers.Abstract;
using RetAil.Dal.Providers.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.Configure<SecretOptions>(builder.Configuration.GetSection("SecretOptions"));
builder.Services.AddScoped<IUserProvider, UserProvider>();
builder.Services.AddDbContext<ApplicationContext>(x => 
    x.UseInMemoryDatabase(builder.Configuration.GetConnectionString("InMemory")!));
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