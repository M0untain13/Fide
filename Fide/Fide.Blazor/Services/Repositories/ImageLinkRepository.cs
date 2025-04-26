using Fide.Blazor.Data;
using Fide.Blazor.Services.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Fide.Blazor.Services.Repositories;

public class ImageLinkRepository(ApplicationDbContext context) : EntityRepository<ImageLink>(context)
{
    protected override DbSet<ImageLink> DbSet => context.ImageLinks;

    protected override IQueryable<ImageLink> Include(IQueryable<ImageLink> query)
    {
        return query.Include(i => i.User);
    }
}
