using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ToDoMinimalAPI.Auth;
using ToDoMinimalAPI.Context;
using ToDoMinimalAPI.ToDo;

var builder = WebApplication.CreateBuilder(args);

var authSettings = builder.Configuration.GetSection("AuthSettings").Get<AuthSettings>();
builder.Services.AddSingleton(authSettings);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApiContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("ApiContext")));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IToDoService, ToDoService>();
builder.Services.AddScoped<IUserContextService, UserContextService>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(config =>
    {
        config.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Key)),
            ValidAudience = authSettings.Audience,
            ValidIssuer = authSettings.Issuer
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

AuthRequests.RegisterEndpoints(app);
ToDoRequests.RegisterEndpoints(app);

app.UseAuthentication();

app.UseAuthorization();

app.Run();
