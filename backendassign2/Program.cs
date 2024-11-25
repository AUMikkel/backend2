using System.Security.Claims;
using backendassign2;
using backendassign2.Services;
using backendassign2.Swashbuckle;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using backendassign2.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMvc();
builder.Services.AddScoped<TokenService>();
var conn = builder.Configuration.GetConnectionString("DefaultConnection") ??
           Environment.GetEnvironmentVariable("DefaultConnection"); // Check if environment variable is set
Console.WriteLine("Connection string: " + conn);
var databaseName = Environment.GetEnvironmentVariable("DatabaseName") ?? "Assignment3"; // Default if not set
// Replace placeholder with actual database name in connection string.
conn = conn.Replace("{DatabaseName}", databaseName);
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.AddSingleton<MongoLogService>();

builder.Services.AddDbContext<dbcontext>(options =>
    options.UseSqlServer(conn, sqlServerOptions =>
        sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 5, // Number of retry attempts
            maxRetryDelay: TimeSpan.FromSeconds(30), // Max delay between retries
            errorNumbersToAdd: null // Additional SQL error codes to consider transient
        )));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.SchemaFilter<PriceValidationSchemaFilter>();
    //c.ParameterFilter<SortColumnFilter>();
    //c.ParameterFilter<SortOrderFilter>();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
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
            Array.Empty<string>()
        }
    });
});

builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
    
});



// Register DbContext

// Register Identity
builder.Services.AddIdentity<ApiUser, IdentityRole>(option =>
    {
        option.Password.RequireDigit = false;
        option.Password.RequireLowercase = false;
        option.Password.RequireUppercase = false;
        option.Password.RequireNonAlphanumeric = true;
        option.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<dbcontext>();
// Add the authentication service
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
        options.DefaultChallengeScheme =
        options.DefaultForbidScheme =
        options.DefaultScheme =
        options.DefaultSignInScheme =
        options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(
                    builder.Configuration["JWT:SigningKey"]))
        };
    });

// Add the authorization service
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManagerAccess", policy =>
                    policy.RequireClaim(ClaimTypes.Role, "Manager"));

});


var app = builder.Build();

//Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<dbcontext>();
    context.Database.EnsureCreated();
    //context.Seed();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();