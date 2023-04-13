using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RetAil;
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
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddDbContext<ApplicationContext>(x => 
    x.UseInMemoryDatabase(builder.Configuration.GetConnectionString("InMemory")!));

#region Jwt Configuration

var secrets = builder.Configuration.GetSection("SecretOptions");

var key = Encoding.ASCII.GetBytes(secrets.GetValue<string>("JWTSecret")!);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

#endregion

ConfigureServicesSwagger.ConfigureServices(builder.Services);
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