using backendassign2;
using backendassign2.Services;
using backendassign2.Swashbuckle;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
var databaseName = "Assignment2";
// Replace placeholder with actual database name in connection string.
conn = conn.Replace("{DatabaseName}", databaseName);
builder.Services.AddMvc();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mys API", Version = "v1" });
    c.SchemaFilter<PriceValidationSchemaFilter>();
});


// Register DbContext
builder.Services.AddDbContext<dbcontext>(options =>
    options.UseSqlServer(conn));
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

app.UseAuthorization();

app.MapControllers();

app.Run();