using Fide.Blazor.Data;
using Fide.Blazor.Services.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fide.Blazor.Tests;

public class RepositoryTests
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
                });
        });
        _host = builder.Build();
    }

    [TearDown]
    public void TearDown()
    {
        _host?.Dispose();
    }

    [Test]
    public async Task GetAsync_CallsFindAsync()
    {
        // Arrange
        var entity = new ImageLink();
        using var context = _host.Services.GetRequiredService<ApplicationDbContext>();
        var repository = new Repository<ImageLink>(context);
        await repository.AddAsync(entity);
        context.SaveChanges();

        // Act
        var result = await repository.FindAsync(i => i.Id == entity.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.ToList(), Has.Count.EqualTo(1));
            Assert.That(result.First(), Is.EqualTo(entity));
        });
    }

    [Test]
    public async Task GetAllAsync_CallsGetAllAsync()
    {
        // Arrange
        var entity = new ImageLink();
        using var context = _host.Services.GetRequiredService<ApplicationDbContext>();
        var repository = new Repository<ImageLink>(context);
        await repository.AddAsync(entity);
        context.SaveChanges();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.ToList(), Has.Count.EqualTo(1));
            Assert.That(result.First(), Is.EqualTo(entity));
        });
    }

    [Test]
    public async Task GetAllAsync_CallsGetAsync()
    {
        // Arrange
        var entity = new ImageLink();
        using var context = _host.Services.GetRequiredService<ApplicationDbContext>();
        var repository = new Repository<ImageLink>(context);
        await repository.AddAsync(entity);
        context.SaveChanges();

        // Act
        var result = await repository.GetAsync(entity.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(entity));
        });
    }

    [Test]
    public async Task Remove_CallsRemove()
    {
        // Arrange
        var entity = new ImageLink();
        using var context = _host.Services.GetRequiredService<ApplicationDbContext>();
        var repository = new Repository<ImageLink>(context);
        await repository.AddAsync(entity);
        context.SaveChanges();

        // Act
        repository.Remove(entity);
        context.SaveChanges();
        var result = await repository.GetAsync(entity.Id);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task Update_CallsContextUpdate()
    {
        // Arrange
        var entity = new ImageLink()
        {
            OriginalName = "foo",
        };
        using var context = _host.Services.GetRequiredService<ApplicationDbContext>();
        var repository = new Repository<ImageLink>(context);
        await repository.AddAsync(entity);
        context.SaveChanges();

        // Act
        entity.OriginalName = "bar";
        repository.Update(entity);
        context.SaveChanges();

        // Assert
        Assert.That(entity.OriginalName, Is.EqualTo("bar"));
    }
}