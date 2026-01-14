using AISDproject.Models;

namespace AISDproject.Repositeries
{
    public interface ITableRepository
    {
        Task<IEnumerable<Table>> GetAllAsync();
        Task<Table> GetByIdAsync(int id);
        Task AddAsync(Table table);
        Task UpdateAsync(Table table);
        Task DeleteAsync(int id);
    }
}
