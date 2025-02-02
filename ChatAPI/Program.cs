using ChatAPI;
using ChatAPI.Data;
using ChatAPI.Data.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSignalR();


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


builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);


//authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie("Cookie");
//builder.Services.UseAuthentication();

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

app.UseCors("frontEnd");
app.MapHub<ChatHub>("/chat");

app.Run();
