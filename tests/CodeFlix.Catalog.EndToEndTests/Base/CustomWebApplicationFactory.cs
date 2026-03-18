using CodeFlix.Infra.Data.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeFlix.Catalog.EndToEndTests.Base;

public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup>
    where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<CodeFlixCatalogDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<CodeFlixCatalogDbContext>(options =>
            {
                options.UseInMemoryDatabase("e2e-tests-db");
            });
        });
    }
}
