using Amazon.S3;
using Fide.Blazor.Components;
using Fide.Blazor.Data;
using Fide.Blazor.Extensions;
using Fide.Blazor.Services;
using Fide.Blazor.Services.AnalysisProxy;
using Fide.Blazor.Services.Data.UnitOfWork;
using Fide.Blazor.Services.EmailSender;
using Fide.Blazor.Services.FileStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Fide.Blazor;

// TODO: много где надо исправить хардкод

public class Program
{
    public static void Main(string[] args)
    {
        var app = Build(args);
        ConfigureApp(app);
        TryConnectAndRun(app);
    }

    private static void TryConnectAndRun(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var timeout = 5000;
        while (!context.Database.CanConnect())
        {
            Console.WriteLine("Try connect to database...");
            Thread.Sleep(timeout);
        }
        Console.WriteLine("Database is connected");
        app.Run();
    }

    private static WebApplication Build(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureBuilder(builder);
        return builder.Build();
    }

    private static void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Configuration.AddJsonFile("appsettings.Local.json");

        // Дефолтные настройки ASP.NET с аутентификацией из под коробки
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
        builder.Services
            .AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        // Конфиги
        builder.Services.Configure<S3Options>(builder.Configuration.GetSection("S3"));
        builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("SMTP"));
        builder.Services.Configure<AomacaOptions>(builder.Configuration.GetSection("Aomaca"));

        // База данных
        builder.Services
            .AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            )
            .AddScoped<IUnitOfWork>(provider =>
            {
                var context = provider.GetRequiredService<ApplicationDbContext>();
                return new UnitOfWork(context);
            });

        // Почтовый клиент
        builder.Services
            .AddSingleton<IEmailSender<ApplicationUser>, ApplicationUserEmailSender>()
            .AddSingleton<IEmailSender, SmtpEmailSender>();

        // Хранилище файлов
        builder.Services.AddSingleton<IAmazonS3>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<S3Options>>().Value;
            var config = new AmazonS3Config
            {
                ServiceURL = options.ServiceURL,
                ForcePathStyle = options.ForcePathStyle
            };
            return new AmazonS3Client(
                options.AccessKey,
                options.SecretKey,
                config
            );
        });
        builder.Services.AddScoped<IFileStorage, S3Service>();

        // Прокси
        builder.Services
            .AddSingleton<IAnalysisProxy, AomacaProxy>();
    }

    private static void ConfigureApp(WebApplication app)
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
}
