using backendassign2;
using backendassign2.Services;
using backendassign2.Swashbuckle;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using backendassign2.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMvc();
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
var databaseName = "Assignment2";
// Replace placeholder with actual database name in connection string.
conn = conn.Replace("{DatabaseName}", databaseName);


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mys API", Version = "v1" });
    c.SchemaFilter<PriceValidationSchemaFilter>();
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