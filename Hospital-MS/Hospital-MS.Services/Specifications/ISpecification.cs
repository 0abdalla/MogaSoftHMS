using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.Specifications
{
    public interface ISpecification<T> where T : class
    {
        Expression<Func<T, bool>>? Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }

        Expression<Func<T, object>>? OrderBy { get; }
        Expression<Func<T, object>>? OrderByDescending { get; }

        int Skip { get; }
        int Take { get; }
        bool IsPaginationEnabled { get; }
        bool AsNoTracking { get; }
    }
}
