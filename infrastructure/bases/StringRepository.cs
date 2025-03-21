public class StringRepository<TEntity> : IStringRepository<TEntity>
    where TEntity : StringEntity
{
    protected readonly AplicationDbContext Db;
    protected readonly DbSet<TEntity> DbSet;

    public DbSet<LogAcao> Audits { get; set; }

    public StringRepository(AplicationDbContext context)
    {
        Db = context;
        DbSet = Db.Set<TEntity>();
    }

    public virtual IQueryable<TEntity> Set()
    {
        return DbSet;
    }

    public TEntity GetById(string id)
    {
        return GetById(id, true);
    }

    public TEntity GetById(string id, bool tracking)
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

    public virtual async Task<TEntity> GetByIdAsync(string id, params string[] includes)
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

    Task IStringRepository<TEntity>.AddRangeAsync(IEnumerable<TEntity> obj)
    {
        return DbSet.AddRangeAsync(obj);
    }

    public virtual Task<TEntity> GetByIdAsync(string id)
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

    public virtual IQueryable<LookupStringEntity> GetLookup()
    {
        return DbSet
            .Select(s => new LookupStringEntity { Key = s.Key, Value = s.Value, ParentKey = s.ParentKey })
            .OrderBy(o => o.Value);
    }

    public virtual void Update(TEntity obj)
    {
        DbSet.Update(obj);
    }

    public virtual void Remove(string id)
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