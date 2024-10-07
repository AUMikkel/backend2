using backendassign2;
using backendassign2.Services;
using backendassign2.Swashbuckle;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
//var databaseName = builder.Configuration["DatabaseSettings:DatabaseName"];
var databaseName = "Assignment2";
// Replace placeholder with actual database name
conn = conn.Replace("{DatabaseName}", databaseName);
builder.Services.AddMvc();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mys API", Version = "v1" });
    c.SchemaFilter<PriceValidationSchemaFilter>();
});


// Register DbContext with the dependency injection container
builder.Services.AddDbContext<dbcontext>(options =>
    options.UseSqlServer(conn));
var app = builder.Build();
//tes
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

app.UseAuthorization();

app.MapControllers();

app.Run();