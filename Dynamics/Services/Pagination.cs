using Microsoft.EntityFrameworkCore;

namespace Dynamics.Services;

public class Pagination : IPagination
{
    public IQueryable<T> ToQueryable<T>(List<T> list) where T : class
    {
        return list.AsQueryable();
    }

    public List<T> PaginationMethod<T>(IQueryable<T> query, int pageNumber, int pageSize) where T : class
    {
        return query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }
}