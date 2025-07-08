using IntegrationTests.Common.Settings;

namespace IntegrationTests.Models;

class UnitOfWork(TodoContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}