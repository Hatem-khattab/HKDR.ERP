using HKDR.API.Services;
using HKDR.API.Services.Auth;
using HKDR.API.Services.HR;
using HKDR.API.Services.HrDashBoard;
using HKDR.Common.Constants;
using HKDR.DomainEntities.Entities.Core;
using HKDR.Infrastructure.Abstractions;
using HKDR.Infrastructure.Data;
using HKDR.Infrastructure.SeederData;
using HKDR.Repository.IRepository;
using HKDR.Repository.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ERPDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Fixed Identity Setup
builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequiredLength = 6;
        options.User.RequireUniqueEmail = true;
    })
    .AddRoles<IdentityRole>()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddUserManager<UserManager<ApplicationUser>>()
    .AddSignInManager<SignInManager<ApplicationUser>>()
    .AddEntityFrameworkStores<ERPDbContext>()
    .AddDefaultTokenProviders();

// Authentication with JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdmin", policy =>
        policy.RequireRole(AppRoles.Admin, AppRoles.SuperAdmin))
    .AddPolicy("RequireSuperAdmin", policy =>
        policy.RequireRole(AppRoles.SuperAdmin));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentCompanyService, CurrentCompanyService>();
builder.Services.AddScoped<IPayrollRepository, PayrollRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHrDashboardService, HrDashboardService>();
builder.Services.AddScoped<IPayrollTransactionRepository, PayrollTransactionRepository>();
builder.Services.AddScoped<IPayrollService, PayrollService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUI",
        policy => policy
            .WithOrigins("https://localhost:7109")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
    );
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "HKDR API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
// DIAGNOSTIC: Check if services are registered
Console.WriteLine("=== DIAGNOSTIC: Checking Service Registration ===");

var services = builder.Services;
var userManagerDescriptor = services.FirstOrDefault(s =>
    s.ServiceType == typeof(UserManager<ApplicationUser>));
Console.WriteLine($"UserManager<ApplicationUser> registered: {userManagerDescriptor != null}");

var roleManagerDescriptor = services.FirstOrDefault(s =>
    s.ServiceType == typeof(RoleManager<IdentityRole>));
Console.WriteLine($"RoleManager<IdentityRole> registered: {roleManagerDescriptor != null}");

// Build a temporary service provider to test
var tempProvider = services.BuildServiceProvider();
try
{
    var userManager = tempProvider.GetRequiredService<UserManager<ApplicationUser>>();
    Console.WriteLine("✓ UserManager<ApplicationUser> can be resolved");
}
catch (Exception ex)
{
    Console.WriteLine($"✗ UserManager<ApplicationUser> resolution failed: {ex.Message}");
}

try
{
    var roleManager = tempProvider.GetRequiredService<RoleManager<IdentityRole>>();
    Console.WriteLine("✓ RoleManager<IdentityRole> can be resolved");
}
catch (Exception ex)
{
    Console.WriteLine($"✗ RoleManager<IdentityRole> resolution failed: {ex.Message}");
}



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Run seeders only once
await DbSeeder.SeedAsync(app.Services);

app.UseHttpsRedirection();
app.UseCors("AllowUI");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();