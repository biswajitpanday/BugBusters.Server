using System.Linq.Expressions;
using DotNetCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BugBusters.Server.Core.Interfaces.Repositories;
using BugBusters.Server.Core.Entities;
using BugBusters.Server.Repository.DatabaseContext;

namespace BugBusters.Server.Repository.Base;

public class BaseRepository<T> : EFRepository<T>, IBaseRepository<T> where T : BaseEntity
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;
    private readonly IQueryable<T?> _queryable;

    public BaseRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
        _queryable = _context.QuerySet<T?>().Where(x => !x!.IsDeleted);
    }

    #region Commands

    public new void Add(T item) => _dbSet.Add(item);

    //public new async Task AddAsync(T item) => await _dbSet.AddAsync(item);

    //public new void AddRange(IEnumerable<T> items) => _dbSet.AddRange(items);

    //public new async Task AddRangeAsync(IEnumerable<T> items) => await _dbSet.AddRangeAsync(items);

    public void HardDelete(object key)
    {
        var entity = _dbSet.Find(key);
        if (entity == null)
            return;
        _dbSet.Remove(entity);
    }
    public void HardDelete(Expression<Func<T, bool>> where)
    {
        var queryable = _dbSet.Where(where);
        if (!queryable.Any())
            return;
        _dbSet.RemoveRange(queryable);
    }
    public Task HardDeleteAsync(object key) => Task.Run(() => HardDelete(key));
    public Task HardDeleteAsync(Expression<Func<T, bool>> where) => Task.Run(() => HardDelete(where));


    public void SoftDelete(object key)
    {
        var entity = _dbSet.Find(key);
        if (entity == null)
            return;
        entity.IsDeleted = true;
        _context.Entry(entity).State = EntityState.Modified;
    }
    public void SoftDelete(Expression<Func<T, bool>> where)
    {
        var queryable = _dbSet.Where(where);
        if (!queryable.Any())
            return;
        foreach (var entity in queryable)
        {
            entity.IsDeleted = true;
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
    public Task SoftDeleteAsync(object key) => Task.Run(() => SoftDelete(key));
    public Task SoftDeleteAsync(Expression<Func<T, bool>> where) => Task.Run(() => SoftDelete(where));

    public new void Update(T item)
    {
        var entity = _dbSet.Find(_context.PrimaryKeyValues<T>(item));
        if (entity == null)
            return;
        _context.Entry(entity).State = EntityState.Detached;
        _context.Update(item);
    }

    public new Task UpdateAsync(T item) => Task.Run(() => Update(item));

    public new void UpdatePartial(object item)
    {
        var entity = _dbSet.Find(_context.PrimaryKeyValues<T>(item));
        if (entity == null)
            return;
        var entityEntry = _context.Entry(entity);
        entityEntry.CurrentValues.SetValues(item);
        foreach (var navigation in entityEntry.Metadata.GetNavigations())
        {
            if (!navigation.IsOnDependent && !navigation.IsCollection &&
                navigation.ForeignKey.IsOwnership)
            {
                var property = item.GetType().GetProperty(navigation.Name);
                if (property != null)
                {
                    var obj = property.GetValue(item, null);
                    if (obj != null)
                        entityEntry.Reference(navigation.Name).TargetEntry?.CurrentValues.SetValues(obj);
                }
            }
        }
    }

    public new Task UpdatePartialAsync(object item) => Task.Run(() => UpdatePartial(item));

    //public new void UpdateRange(IEnumerable<T> items) => _dbSet.UpdateRange(items);

    //public new Task UpdateRangeAsync(IEnumerable<T> items) => Task.Run((Action)(() => UpdateRange(items)));


    #endregion


    #region Queries

    public new bool Any() => _queryable!.Any();

    public new bool Any(Expression<Func<T, bool>> where) => _queryable!.Any(@where);

    public new async Task<bool> AnyAsync() => await _queryable!.AnyAsync();

    public new async Task<bool> AnyAsync(Expression<Func<T, bool>> where) => await _queryable!.AnyAsync(@where);

    public new long Count() => _queryable!.LongCount();

    public new long Count(Expression<Func<T, bool>> where) => _queryable!.LongCount(@where);

    public new async Task<long> CountAsync() => await _queryable!.LongCountAsync();

    public new async Task<long> CountAsync(Expression<Func<T, bool>> where) => await _queryable!.LongCountAsync(@where);

    public T? Get(Guid key) => _context.DetectChangesLazyLoading(false).Set<T>().Find(key);

    public async Task<T?> GetAsync(Guid key) => await _queryable.Where(x => x!.Id == key).SingleOrDefaultAsync();

    public new IEnumerable<T> List() => _queryable!.ToList();

    public new async Task<IEnumerable<T>> ListAsync() => await _queryable!.ToListAsync().ConfigureAwait(false);


    #endregion

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    public void Save() => _context.SaveChanges();
}