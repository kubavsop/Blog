using System.Text;
using Blog.API.Data;
using Blog.API.Middlewares;
using Blog.API.Services;
using Blog.API.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<ITokenProvider, TokenProvider>();
builder.Services.AddSingleton<DatabaseMigrator>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<ICommunityService, CommunityService>();
builder.Services.AddScoped<ICommunityAccessService, CommunityAccessService>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    var connection = builder.Configuration.GetConnectionString("Redis");
    options.Configuration = connection;
});
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var key = builder.Configuration.GetValue<string>("AppSettings:SecretKey")!;
var issuer = builder.Configuration.GetValue<string>("AppSettings:Issuer")!;
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = false
    };
});

var app = builder.Build();

var dbContext = app.Services.CreateScope().ServiceProvider.GetService<AppDbContext>();
var migrator = app.Services.GetRequiredService<DatabaseMigrator>();

if (dbContext != null && dbContext.Database.GetPendingMigrations().Any())
{
    dbContext.Database.EnsureCreated();
    dbContext.Database.Migrate();
    migrator.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<TokenManagerMiddleware>();

app.MapControllers();

app.Run();