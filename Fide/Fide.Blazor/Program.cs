using Fide.Blazor.Components;
using Fide.Blazor.Data;
using Fide.Blazor.Extensions;
using Fide.Blazor.Services;
using Fide.Blazor.Services.AnalysisProxy;
using Fide.Blazor.Services.EmailSender;
using Fide.Blazor.Services.S3Proxy;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("Fide")
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
            .AddSingleton<IS3Proxy, S3ProxyStub>()
            .AddSingleton(_ => new FideEnvironment(isDebug: true));
    }

    private static void ConfigureBuilderRelease(WebApplicationBuilder builder)
    {
        Console.WriteLine("Используется RELEASE сборка");

        var connectionString = GetRequiredEnvironmentVariable("CONNECTION_STRING");
        var smtpHost = GetRequiredEnvironmentVariable("SMTP_HOST");
        var smtpPort = Convert.ToInt32(GetRequiredEnvironmentVariable("SMTP_PORT"));
        var smtpEmail = GetRequiredEnvironmentVariable("SMTP_EMAIL");
        var smtpPassword = GetRequiredEnvironmentVariable("SMTP_PASSWORD");
        var minioHost = GetRequiredEnvironmentVariable("MINIO_HOST");
        var minioUser = GetRequiredEnvironmentVariable("MINIO_ROOT_USER");
        var minioPassword = GetRequiredEnvironmentVariable("MINIO_ROOT_PASSWORD");
        var aomacaHost = GetRequiredEnvironmentVariable("AOMACA_HOST");

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


        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString)
        );

        builder.Services
            .AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        builder.Services
            .AddSingleton<IEmailSender<ApplicationUser>, ApplicationUserEmailSender>()
            .AddSingleton<IEmailSender>(provider =>
            {
                return new SmtpEmailSender(smtpHost, smtpPort, smtpEmail, smtpPassword);
            })
            .AddMinio(configureClient =>
            {
                configureClient
                    .WithEndpoint(minioHost)
                    .WithCredentials(minioUser, minioPassword)
                    .Build();
            })
            .AddSingleton<IAnalysisProxy>(provider =>
            {
                var httpClient = new HttpClient
                {
                    BaseAddress = new Uri($"http://{aomacaHost}")
                };
                var logger = provider.GetRequiredService<ILogger<AomacaProxy>>();
                return new AomacaProxy(httpClient, logger);
            })
            .AddSingleton<IS3Proxy, MinioProxy>()
            .AddSingleton(_ => new FideEnvironment(isDebug: false));

        builder.WebHost.UseUrls("http://[::]:80");
    }

    private static void Configure(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.MapAdditionalIdentityEndpoints();
    }

    private static string GetRequiredEnvironmentVariable(string name)
        => Environment.GetEnvironmentVariable(name) 
            ?? throw new NullReferenceException($"{name} is missing");
}
