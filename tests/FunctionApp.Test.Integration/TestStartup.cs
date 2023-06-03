using Core.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;

namespace FunctionApp.Test;

public class TestStartup: Startup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        base.Configure(builder);

        builder.Services.AddTransient<UserTrigger>();
    }
}
