using Fide.Blazor.Data;
using Fide.Blazor.Services.Data.Repository;
using Fide.Blazor.Services.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fide.Blazor.Tests;

[TestFixture]
public class UnitOfWorkTests
{
    private IHost _host;

    [SetUp]
    public void Setup()
    {
        var builder = new HostBuilder();
        builder.ConfigureServices(services =>
        {
            services
                .AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemory");
                })
                .AddScoped(provider =>
                    new UnitOfWork(provider.GetRequiredService<ApplicationDbContext>())
                );
        });
        _host = builder.Build();
    }

    [TearDown]
    public void TearDown()
    {
        _host?.Dispose();
    }

    [Test]
    public void GetRepository_ReturnsRepositoryForType()
    {
        // Arrange
        var uow = _host.Services.GetRequiredService<UnitOfWork>();

        // Act
        var repository = uow.GetRepository<ImageLink>();

        // Assert
        Assert.That(repository, Is.Not.Null);
        Assert.That(repository, Is.InstanceOf<IRepository<ImageLink>>());
    }

    [Test]
    public async Task Rollback_ResetsAddedEntities()
    {
        // Arrange
        var entity = new ImageLink();
        var uow = _host.Services.GetRequiredService<UnitOfWork>();
        var repository = uow.GetRepository<ImageLink>();
        await repository.AddAsync(entity);

        // Act
        uow.Rollback();

        // Assert
        var images = await repository.FindAsync(i => i.Id == entity.Id);
        Assert.That(images.ToList(), Has.Count.Zero);
    }

    [Test]
    public async Task Rollback_ResetsModifiedEntities()
    {
        // Arrange
        var entity = new ImageLink()
        {
            OriginalName = "Foo",
        };
        var uow = _host.Services.GetRequiredService<UnitOfWork>();
        var repository = uow.GetRepository<ImageLink>();
        await repository.AddAsync(entity);
        await uow.CommitAsync();

        // Act
        entity.OriginalName = "Bar";
        uow.Rollback();

        // Assert
        var images = await repository.FindAsync(i => i.Id == entity.Id);
        Assert.Multiple(() =>
        {
            Assert.That(images.ToList(), Has.Count.EqualTo(1));
            Assert.That(entity.OriginalName, Is.EqualTo("Foo"));
        });
    }

    [Test]
    public async Task Rollback_ResetsDeletedEntities()
    {
        // Arrange
        var entity = new ImageLink();
        var uow = _host.Services.GetRequiredService<UnitOfWork>();
        var repository = uow.GetRepository<ImageLink>();
        await repository.AddAsync(entity);
        await uow.CommitAsync();

        // Act
        repository.Remove(entity);
        uow.Rollback();

        // Assert
        var images = await repository.FindAsync(i => i.Id == entity.Id);
        Assert.That(images.ToList(), Has.Count.EqualTo(1));
    }
}