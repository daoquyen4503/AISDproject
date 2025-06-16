using AISDproject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AISDproject.Services
{
    public interface ITableService
    {
        Task<IEnumerable<Table>> GetAllTablesAsync();
        Task<Table> GetTableByIdAsync(int id);
        Task CreateTableAsync(Table table);
        Task UpdateTableAsync(Table table);
        Task DeleteTableAsync(int id);
        Task<IEnumerable<SelectListItem>> GetTablesSelectListAsync();
    }
}
