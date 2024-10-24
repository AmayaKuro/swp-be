using Microsoft.EntityFrameworkCore;
using swp_be.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using swp_be.Services;

var builder = WebApplication.CreateBuilder(args);

var BEUrl = builder.Configuration.GetSection("BEUrl").Get<string>();
var FEUrl = builder.Configuration.GetSection("FEUrl").Get<string>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Web API with Jwt Authentication", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Allow CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "cors", policy =>
    {
        policy.WithOrigins(FEUrl).AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionString", System.EnvironmentVariableTarget.User));
});

//Jwt configuration starts here
var jwtSecretKey = Environment.GetEnvironmentVariable("Swp_Secret_key", System.EnvironmentVariableTarget.User) ?? "YSBsb25nIEFzc3MgIHN0cmluZyB0aGF0J3MgbGFzc3QgNjQgY2hhcmFjdGVyPz8/";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = BEUrl,
            ValidAudience = FEUrl,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
        };
    });

builder.Services.AddAuthorizationBuilder().AddPolicy("staff", policy => policy.RequireRole("Staff"));
builder.Services.AddAuthorizationBuilder().AddPolicy("admin", policy => policy.RequireRole("Admin"));
builder.Services.AddAuthorizationBuilder().AddPolicy("customer", policy => policy.RequireRole("Customer"));
builder.Services.AddAuthorizationBuilder().AddPolicy("all", policy => policy.RequireRole("Staff", "Admin", "Customer"));

builder.Services.AddScoped<BatchService>();
builder.Services.AddScoped<FeedbackService>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();   
    app.UseSwaggerUI();
}

app.MapControllers();

// THIS ORDER OF APP IS NECCESARY
app.UseCors("cors");

app.UseAuthentication();
app.UseAuthorization();


app.Run();
