using Amazon.S3;
using Fide.Blazor.Components;
using Fide.Blazor.Data;
using Fide.Blazor.Extensions;
using Fide.Blazor.Services;
using Fide.Blazor.Services.AnalysisProxy;
using Fide.Blazor.Services.EmailSender;
using Fide.Blazor.Services.FileStorage;
using Fide.Blazor.Services.ImageManager;
using Fide.Blazor.Services.Repositories;
using Fide.Blazor.Services.Repositories.Base;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Fide.Blazor;

public class Program
{
    public static void Main(string[] args)
    {
        var app = Build(args);
        ConfigureApp(app);
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

        // Дефолтные настройки проекта
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
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        );

        // Почтовый клиент
        builder.Services
            .AddSingleton<IEmailSender<ApplicationUser>, ApplicationUserEmailSender>()
            .AddSingleton<IEmailSender, SmtpEmailSender>();

        // Репозитории
        builder.Services
            .AddTransient<IEntityRepository<ImageLink>, ImageLinkRepository>()
            .AddTransient<IUserRepository<ApplicationUser>, UserRepository>();

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

        // Остальное
        builder.Services
            .AddSingleton<IAnalysisProxy, AomacaProxy>()
            .AddScoped<IImageManager, ImageManager>();
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
