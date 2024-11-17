using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<T> where T : BaseEntity
    {

        public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
        {

            if (spec.Criteria != null)
            {
                query=query.Where(spec.Criteria);// x=> x.?==?
            }

            if (spec.OrderBy != null)
            {

                query = query.OrderBy(spec.OrderBy);
            } 
            
            if (spec.OrderByDescending != null)
            {

                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.IsDistinct)
            {
                query = query.Distinct();
            }

            if (spec.IsPagingEnabled)
            {
               query= query.Skip(spec.Skip).Take(spec.Take);
            }

            
            query=spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            query=spec.IncludeStrings.Aggregate(query,(current,include)=>current.Include(include));
            return query;
        
        }
        public static IQueryable<TResult> GetQuery<TSpec,TResult>(IQueryable<T> query,
            ISpecification<T,TResult> spec)
        {

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);// x=> x.?==?
            
            }

            if (spec.OrderBy != null)
            {

                query = query.OrderBy(spec.OrderBy);
            
            }

            if (spec.OrderByDescending != null)
            {

                query = query.OrderByDescending(spec.OrderByDescending);
            
            }

            // Use spec.Select to transform query to TResult
            IQueryable<TResult> selectQuery = null;

            if (spec.Select != null)
            {
                selectQuery = query.Select(spec.Select); // Transforming T to TResult
            }


            if (spec.IsDistinct&&selectQuery!=null)
            {
                selectQuery = selectQuery.Distinct();
            }
        
            if (spec.IsPagingEnabled && selectQuery != null)
            {
              selectQuery=  selectQuery.Skip(spec.Skip).Take(spec.Take);
            }
            // Return selectQuery if successful; otherwise, attempt a cast with caution
            return selectQuery ?? throw new InvalidOperationException("Select expression required for TResult projection.");
       
        }

    }


}
