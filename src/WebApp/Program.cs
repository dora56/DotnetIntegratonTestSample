var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options =>
    options
        .UseSqlServer(
            builder.Configuration.GetConnectionString("DatabaseConnection"),
            options => options.EnableRetryOnFailure(5)) // 接続の回復性
        .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)); // 追跡ありクエリ


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
