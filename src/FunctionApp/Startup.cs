[assembly: FunctionsStartup(typeof(FunctionApp.Startup))]

namespace FunctionApp;

public class Startup: FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        
        builder.Services
            .AddHttpContextAccessor()
            .AddDatabaseService(configuration)
            .AddAzureService(configuration);
        
        builder.Services.AddScoped<IUserRepository, UserRepository>();
    }
}

public static class StartupExtension
{
    public static IServiceCollection AddDatabaseService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options
                .UseSqlServer(
                    configuration.GetConnectionString("DatabaseConnection"),
                    optionsBuilder => optionsBuilder.EnableRetryOnFailure(5)) // 接続の回復性
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)); // 追跡ありクエリ
        return services;
    }
    
    public static IServiceCollection AddAzureService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // add service account
        // var azureStorageSection = configuration.GetSection("Azure:Storage");
        
        // services.AddOptions<StorageOptions>()
        //     .Configure<IConfiguration>((settings, _) => azureStorageSection.Bind(settings));
        
        var connectionString = configuration.GetConnectionString("StorageConnection");
        services.AddAzureClients(builder =>
        {
            builder.AddQueueServiceClient(connectionString);
            builder.AddBlobServiceClient(connectionString);
        });

        return services;
    }
}
