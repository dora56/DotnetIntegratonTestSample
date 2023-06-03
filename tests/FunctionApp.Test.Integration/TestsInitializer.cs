using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;

namespace FunctionApp.Test;

public class TestsInitializer
{
    public TestsInitializer()
    {
        var host = new HostBuilder()
            .ConfigureWebJobs(builder => builder.UseWebJobsStartup(typeof(TestStartup), new WebJobsBuilderContext(), NullLoggerFactory.Instance))
            .Build();

        ServiceProvider = host.Services;
    }

    public IServiceProvider ServiceProvider { get; }
}
