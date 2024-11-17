using Core.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        private readonly Expression<Func<T, bool>>? criteria;
        public BaseSpecification(Expression<Func<T, bool>> _criteria)
        {
            this.criteria = _criteria;
        }
        protected BaseSpecification() : this(null)
        {

        }

        public Expression<Func<T, bool>>? Criteria => criteria;

        public Expression<Func<T, object>>? OrderBy { get;private set; }

        public Expression<Func<T, object>>? OrderByDescending { get; private set; }

        public bool IsDistinct { get; private set; }

        public int Take { get;private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnabled { get;private set; }

        public List<Expression<Func<T, object>>> Includes { get; private set; } = [];

        public List<string> IncludeStrings { get; private set; } = [];

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        { 
        
            OrderBy= orderByExpression;

        }

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
        protected void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);//for then include 
        }
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {

            OrderByDescending = orderByDescExpression;

        }

        protected void ApplyPaging(int skip,int take)
        {

            Skip= skip;
            Take= take;
            IsPagingEnabled = true;
        }
        protected void ApplyDistinct()
        {
            IsDistinct = true;
        }
        public IQueryable<T> ApplyCriteria(IQueryable<T> query)
        {
            if (criteria != null)
            {

                query= query.Where(criteria);
            }

            return query;

        }
    
    }

    public class BaseSpecification<T, TResult>(Expression<Func<T, bool>> criteria)
        : BaseSpecification<T>(criteria), ISpecification<T, TResult>
    {

        protected BaseSpecification() : this(null!)
        {

        }

        public Expression<Func<T, TResult>>? Select {get;private set;}

        protected void AddSelect(Expression<Func<T, TResult>> selectExpression)
        {
           Select= selectExpression;
        }

    }

}
