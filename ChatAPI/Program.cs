using ChatAPI;
using ChatAPI.Data;
using ChatAPI.Options;
using ChatAPI.Services;
using ChatAPI.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSignalR();


// setting up options
builder.Services.Configure<DatabaseOptions>(
    builder.Configuration.GetSection(DatabaseOptions.DatabaseOption));
builder.Services.Configure<JWTOptions>(
    builder.Configuration.GetSection(JWTOptions.JWTOption));

builder.Services.AddCors(option =>
{
    option.AddPolicy("frontEnd", builder => 
    { 
        builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials(); 
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//mongo setup
builder.Services.AddSingleton<MongoDBService>();
builder.Services.AddScoped<IUserService, UserService>();

//passwordhasher
builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();

//builder.Services.AddControllers()
//    .AddJsonOptions(
//        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);


//authentication
builder.Services.AddAuthorization();
var jwtConfig = builder.Configuration.GetSection(JWTOptions.JWTOption).Get<JWTOptions>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
    {
        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret)),
            ValidIssuer = jwtConfig.Issuer,
            ValidAudience = jwtConfig.Audience,
            ClockSkew = TimeSpan.Zero,
        };

    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


//authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("frontEnd");
app.MapHub<ChatHub>("/chat");

app.Run();
