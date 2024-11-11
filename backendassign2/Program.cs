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

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMvc();
builder.Services.AddScoped<TokenService>();
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
var databaseName = "Assignment2";
// Replace placeholder with actual database name in connection string.
conn = conn.Replace("{DatabaseName}", databaseName);
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.AddSingleton<MongoLogService>();


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
builder.Services.AddDbContext<dbcontext>(options =>
    options.UseSqlServer(conn));
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
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MatchFullNamePolicy", policy =>
        policy.RequireAssertion(async context =>
        {
            var fullNameClaim = context.User.FindFirst("FullName");
            if (fullNameClaim == null)
            {
                Console.WriteLine("FullName claim not found");
                return false;
            }

            Console.WriteLine($"FullName claim: {fullNameClaim.Value}");

            if (context.Resource is HttpContext httpContext &&
                httpContext.Request.RouteValues.TryGetValue("cookId", out var routeCookId))
            {
                Console.WriteLine($"Route cookId: {routeCookId}");

                if (int.TryParse(routeCookId.ToString(), out var cookId))
                {
                    // Get the cook's full name from the database
                    var dbContext = httpContext.RequestServices.GetRequiredService<dbcontext>();
                    var cook = await dbContext.Cooks.FindAsync(cookId);

                    if (cook != null)
                    {
                        Console.WriteLine($"Database fullName: {cook.FullName}");
                        return string.Equals(fullNameClaim.Value, cook.FullName, StringComparison.OrdinalIgnoreCase);
                    }
                }
            }

            return false;
        }));
});

var app = builder.Build();

//Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<dbcontext>();
    context.Database.EnsureCreated();
    context.Seed();
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