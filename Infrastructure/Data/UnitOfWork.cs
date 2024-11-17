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
            var type = typeof(TEntity).Name;

            // Only log when creating a new repository instance
            var repository = (IGenericRepository<TEntity>)_repositories.GetOrAdd(type, t =>
            {
                Console.WriteLine($"Creating repository for type: {type}");
                var repositoryType = typeof(GenericRepository<>).MakeGenericType(typeof(TEntity));
                return Activator.CreateInstance(repositoryType, context)
                    ?? throw new InvalidOperationException($"Could not create repository instance for {t}");
            });

            // Log the instance hash code to check if it's the same
            Console.WriteLine($"Repository instance hash code: {repository.GetHashCode()}");

            return repository;
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
