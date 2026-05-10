using FarmToTable.Repositories;
using FarmToTable.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register Repositories
builder.Services.AddScoped<IFarmRepository, FarmRepository>();
builder.Services.AddScoped<ICropRepository, CropRepository>();
builder.Services.AddScoped<IHarvestBatchRepository, HarvestBatchRepository>();
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<ITripRepository, TripRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<ITaskRequirementsRepository, TaskRequirementsRepository>();

var app = builder.Build();

// Database connectivity check on startup
using (var scope = app.Services.CreateScope())
{
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var connStr = config.GetConnectionString("DefaultConnection");
    try
    {
        using var conn = new Microsoft.Data.SqlClient.SqlConnection(connStr);
        conn.Open(); // Async inside top-level statements requires async Task Main, but Open() is fine here since it's just a startup check
        app.Logger.LogInformation(" Database connection successful.");
    }
    catch (Exception ex)
    {
        app.Logger.LogCritical(" Cannot connect to database. Check connection string in appsettings.json. Error: {Message}", ex.Message);
        // Do not throw — let the app start so the error page is visible
    }
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
