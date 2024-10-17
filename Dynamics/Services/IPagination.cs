namespace Dynamics.Services;

public interface IPagination
{
    IQueryable<T> ToQueryable<T>(List<T> list) where T : class;
    List<T> PaginationMethod<T>(IQueryable<T> query, int pageNumber, int pageSize) where T : class;
}