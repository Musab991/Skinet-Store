using API.Extensions;
using API.Middleware;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add DbContext
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IProductReposoitory, ProductRepository>();
builder.Services.AddCors();
builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
{
    var connString = builder.Configuration.GetConnectionString("Redis") 
    ?? throw new Exception("Cannot get redis connection string");

    var configuration = ConfigurationOptions.Parse(connString, true);
    return ConnectionMultiplexer.Connect(configuration);
});
builder.Services.AddSingleton<ICartService, CartService>();
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<AppUser>()
    .AddEntityFrameworkStores<StoreContext>();

builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddRepositories(Assembly.GetExecutingAssembly());

var app = builder.Build();

#region Middleware

app.UseMiddleware<ExceptionMiddleware>();
#endregion

#region CORS

//order is important ...(app<<--->mapcontrollers)
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
.WithOrigins("http://localhost:4200", "https://localhost:4200"));

#endregion

// Enable Swagger middleware and Swagger UI in the development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapGroup("api").MapIdentityApi<AppUser>();//api/login(register)//


#region Seed Data
try
{

    using var scope = app.Services.CreateScope();

    var services=scope.ServiceProvider;

    var context=services.GetRequiredService<StoreContext>();

    await context.Database.MigrateAsync();

    await StoreContextSeed.SeedAsync(context);


}
catch(Exception ex)
{

    Console.WriteLine(ex.ToString());
    throw;
}
#endregion

app.Run();
