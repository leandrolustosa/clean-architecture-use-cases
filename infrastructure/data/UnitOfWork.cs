public class UnitOfWork : IUnitOfWork
{        
    private readonly AplicationDbContext context;

    private IDbContextTransaction transaction;

    public UnitOfWork(AplicationDbContext context)
    {
        this.context = context;
    }

    public async Task BeginTransactionAsync()
    {
        if (transaction == null)
        {
            transaction = await context.Database.BeginTransactionAsync();
        }
    }

    public async Task<bool> CommitAsync()
    {
        try
        {
            var rowsAffected = await context.SaveChangesAsync() > 0;
            transaction?.Commit();

            return rowsAffected;
        }
        catch (DbUpdateException updateException)
        {
            Rollback();

            if (updateException.InnerException is OracleException oracleException)
            {
                switch (oracleException.Number)
                {
                    case 2292:                            
                            throw new RestricaoIntegridadeException(oracleException);
                }
            }

            throw;
        }
        catch(Exception ex)
        {
            Rollback();
            throw;
        }
        finally
        {
            transaction?.Dispose();
            transaction = null;
        }
    }

    public void Rollback()
    {
        transaction?.Rollback();
        transaction?.Dispose();
        transaction = null;

        DetachAllEntities();
    }

    private void DetachAllEntities()
    {
        var changedEntriesCopy = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added ||
                        e.State == EntityState.Modified ||
                        e.State == EntityState.Deleted)
            .ToList();

        foreach (var entry in changedEntriesCopy)
            entry.State = EntityState.Detached;
    }

    public void Dispose()
    {
        context.Dispose();
    }
}