using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UnitOfWork(StoreContext context) : IUnitOfWork //With use of Activator 
    {

        private readonly ConcurrentDictionary<string, object> _repositories = new();


        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type=typeof(TEntity).Name;

            return (IGenericRepository<TEntity>)_repositories.GetOrAdd(type, t =>
            {
                var respositoryType = typeof(GenericRepository<>).MakeGenericType(typeof(TEntity));
                return Activator.CreateInstance(respositoryType, context)
                ?? throw new InvalidOperationException(
                    $"Could not create repository instance for{t}");
            });

        }
        public async Task<bool> Complete()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            context.Dispose();
        }

    }
}
