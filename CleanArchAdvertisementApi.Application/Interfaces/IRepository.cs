namespace CleanArchAdvertisementApi.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<string> AddAsync(T entity);
        Task<string> UpdateAsync(T entity);
        Task<string> DeleteAsync(Guid id);
        Task<List<T>> MultiSortAsync(string? search, string? orderByQueryString, DateTime? startDate, DateTime? endDate);
    }
}
