using System.Reflection;
using Business.Extensions;
using Business.Services;
using Business.Services;
using Business.Services;
using Business.Services.Token;
using Business.Services;
using DataAccess;
using DataAccess.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Note.Middleware;
using Business.Services.Market;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GayeBilisim", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
       {
       new OpenApiSecurityScheme
         {
             Reference = new OpenApiReference
             {
                 Type = ReferenceType.SecurityScheme,
                 Id = "Bearer"
             }
         },
         new string[] {}
     }
   });
});

//APPSETTINGS GET OPERATIONS
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);

builder.Services.AddDbContext<AppDbContext>(x =>

    x.UseNpgsql(builder.Configuration["ConnectionStrings:DbConnection"], option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    })
);

//AUTHENTICATION FOR DEFAULT
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
});
//AUTHORIZATION
builder.Services.AddAuthorization(x =>
{
    x.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();
});

//Dependency Injection
builder.Services.AddSingleton<ITokenHandler, Business.Services.Token.TokenHandler>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserDal, UserDal>();
builder.Services.AddTransient<ICategoryDal, CategoryDal>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IArchiveService, ArchiveService>();
builder.Services.AddTransient<IArchiveDal, ArchiveDal>();
builder.Services.AddAutoMapper(typeof(Business.Mapping.MapProfile).GetTypeInfo().Assembly);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IWalletDal, WalletDal>();
builder.Services.AddTransient<IWalletService, WalletService>();
builder.Services.AddTransient<IMarketService, MarketService>();




var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
