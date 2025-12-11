using Hospital_MS.Services.Specifications;
using System.Linq.Expressions;
using LinqKit;

namespace mogaERP.Domain.Specifications;
public class BaseSpecification<T> : ISpecification<T> where T : class
{
    public Expression<Func<T, bool>>? Criteria { get; private set; }
    public List<Expression<Func<T, object>>> Includes { get; } = new();
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }
    public List<string> IncludeStrings { get; } = new List<string>();

    public int Skip { get; private set; }
    public int Take { get; private set; }
    public bool IsPaginationEnabled { get; private set; }
    public bool AsNoTracking { get; private set; }

    protected BaseSpecification() { }

    protected BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    public void AddCriteria(Expression<Func<T, bool>> additionalCriteria)
    {
        if (Criteria is null)
            Criteria = additionalCriteria;

        else
            Criteria = Criteria.And(additionalCriteria);
    }

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
        => Includes.Add(includeExpression);

    protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        => OrderBy = orderByExpression;

    protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        => OrderByDescending = orderByDescExpression;

    protected void ApplyPagination(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPaginationEnabled = true;
    }
    protected virtual void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }

    public void DisablePagination()
    {
        IsPaginationEnabled = false;
    }

    public void ApplyAsNoTracking()
    {
        AsNoTracking = true;
    }
}
