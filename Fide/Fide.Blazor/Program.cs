using Fide.Blazor.Components;
using Fide.Blazor.Components.Account;
using Fide.Blazor.Data;
using Fide.Blazor.Services.AnalysisProxy;
using Fide.Blazor.Services.S3Proxy;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Minio;

namespace Fide.Blazor;

public class Program
{
    public static void Main(string[] args)
    {
        var app = Build(args);
        Configure(app);
        app.Run();
    }

    private static WebApplication Build(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

#if RELEASE
        ConfigureBuilderRelease(builder);
#else
        ConfigureBuilderDebug(builder);
#endif

        return builder.Build();
    }

    private static void ConfigureBuilderDebug(WebApplicationBuilder builder)
    {
        Console.WriteLine("Используется DEBUG сборка");

        builder.Services
            .AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services
            .AddCascadingAuthenticationState()
            .AddScoped<IdentityUserAccessor>()
            .AddScoped<IdentityRedirectManager>()
            .AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

        var connectionString = @"Server=(localdb)\\mssqllocaldb;Database=aspnet-Fide.Blazor-74127606-726e-4155-8f12-ae484fd1d53e;Trusted_Connection=True;MultipleActiveResultSets=true";

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString)
        );

        builder.Services
            .AddDatabaseDeveloperPageExceptionFilter();

        builder.Services
            .AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        builder.Services
            .AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>()
            .AddSingleton<IAnalysisProxy, AnalysisProxyStub>()
            .AddSingleton<IS3Proxy, S3ProxyStub>();
    }

    private static void ConfigureBuilderRelease(WebApplicationBuilder builder)
    {
        Console.WriteLine("Используется RELEASE сборка");

        builder.Services
            .AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services
            .AddCascadingAuthenticationState()
            .AddScoped<IdentityUserAccessor>()
            .AddScoped<IdentityRedirectManager>()
            .AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
            ?? throw new NullReferenceException("CONNECTION_STRING variable is missing");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString)
        );

        builder.Services
            .AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        builder.Services
            .AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>()
            .AddMinio(configureClient =>
            {
                var minioHost = Environment.GetEnvironmentVariable("MINIO_HOST");
                var accessKey = Environment.GetEnvironmentVariable("MINIO_ROOT_USER");
                var secretKey = Environment.GetEnvironmentVariable("MINIO_ROOT_PASSWORD");
                configureClient
                    .WithEndpoint(minioHost)
                    .WithCredentials(accessKey, secretKey)
                    .Build();
            })
            .AddSingleton<IAnalysisProxy>(provider =>
            {
                var aomacaHost = Environment.GetEnvironmentVariable("AOMACA_HOST");
                var httpClient = new HttpClient
                {
                    BaseAddress = new Uri($"http://{aomacaHost}")
                };
                var logger = provider.GetRequiredService<ILogger<AomacaProxy>>();
                return new AomacaProxy(httpClient, logger);
            })
            .AddSingleton<IS3Proxy, MinioProxy>();

        builder.WebHost.UseUrls("http://[::]:80");
    }

    private static void Configure(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        // Add additional endpoints required by the Identity /Account Razor components.
        app.MapAdditionalIdentityEndpoints();
    }
}
