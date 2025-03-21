public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : Entity
{
    protected readonly AplicationDbContext Db;
    protected readonly DbSet<TEntity> DbSet;

    public DbSet<LogAcao> Audits { get; set; }

    public Repository(AplicationDbContext context)
    {
        Db = context;
        DbSet = Db.Set<TEntity>();
    }

    public virtual IQueryable<TEntity> Set()
    {
        return DbSet;
    }

    public TEntity GetById(long id)
    {
        return GetById(id, true);
    }

    public TEntity GetById(long id, bool tracking)
    {
        if (tracking)
        {
            return DbSet.FirstOrDefault(c => c.Id == id);
        }
        else
        { 
            return DbSet.AsNoTracking().FirstOrDefault(c => c.Id == id);
        }
    }

    public virtual async Task<TEntity> GetByIdAsync(long id, params string[] includes)
    {
        var query = GetAll();

        if (includes?.Length > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        query = query.Where(p => p.Id == id);

        return await query.FirstOrDefaultAsync();
    }

    public virtual Task AddAsync(TEntity obj)
    {
        return DbSet.AddAsync(obj);
    }

    Task IRepository<TEntity>.AddRangeAsync(IEnumerable<TEntity> obj)
    {
        return DbSet.AddRangeAsync(obj);
    }

    public virtual Task<TEntity> GetByIdAsync(long id)
    {
        return DbSet.FindAsync(id);
    }

    public virtual IQueryable<TEntity> GetAll()
    {
        return DbSet;
    }

    public IQueryable<TEntity> GetAll(params string[] includes)
    {
        var query = GetAll();

        if (includes?.Length > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return query;
    }

    public virtual IQueryable<LookupEntity> GetLookup()
    {
        return DbSet
            .Select(s => new LookupEntity { Key = s.Key, Value = s.Value, ParentKey = s.ParentKey })
            .OrderBy(o => o.Value);
    }

    public virtual void Update(TEntity obj)
    {
        DbSet.Update(obj);
    }

    public virtual void Remove(long id)
    {
        DbSet.Remove(DbSet.Find(id));
    }

    public virtual void Remove(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<TEntity> obj, IAuditContext auditContext, User user)
    {
        if (typeof(TEntity).IsSubclassOf(typeof(AuditableEntity)))
        {
            obj.Cast<AuditableEntity>().ToList().ForEach(o => { 
                o.AuditContext = auditContext; 
                o.User = user; 
            });
        }
        DbSet.RemoveRange(obj);
    }

    public async Task<int> SaveChangesAsync()
    {            
        return await Db.SaveChangesAsync();
    }

    public void Dispose()
    {
        Db.Dispose();
        GC.SuppressFinalize(this);
    }
    public virtual IQueryable<TEntity> Buscar(Expression<Func<TEntity, bool>> predicate)
    {
        return DbSet.Where(predicate);
    }
}