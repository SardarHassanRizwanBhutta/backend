// Below  lines are also added as part of the registering services with the DI container

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            // ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretKey)
        };

        // Extract roleName claim and set it as User Role
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var userClaims = context.Principal?.Claims;
                var roleClaim = userClaims?.FirstOrDefault(c => c.Type == "roleName")?.Value;

                if (!string.IsNullOrEmpty(roleClaim))
                {
                    var claimsIdentity = (System.Security.Claims.ClaimsIdentity)context.Principal!.Identity!;
                    claimsIdentity.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, roleClaim));
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
// Learning dependency injection - The AddScoped method registers the service with a scoped lifetime, the lifetime of single
//  request
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<UserService>();
    
// builder.Services.AddDbContext<UserContext>(opt =>
//     opt.UseInMemoryDatabase("UserList"));
// services such as DBContext must be registered with the dependency injection (DI) container, these container provides service
// to the controllers
// register the DatabaseContext service inside IService Collection which provides contract for collection of services
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// AddDbContextPool<> - now our DatabaseContext instances(objects) will be resued

builder.Services.AddDbContextPool<DatabaseContext>(opt =>
    opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();