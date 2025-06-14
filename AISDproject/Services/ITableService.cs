using AISDproject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public interface ITableService
{
    Task<IEnumerable<Table>> GetAllTablesAsync(); // <-- dùng hàm này trong Controller
    Task<Table> GetTableByIdAsync(int id);
    Task CreateTableAsync(Table table);
    Task UpdateTableAsync(Table table);
    Task DeleteTableAsync(int id);
    Task<IEnumerable<SelectListItem>> GetTablesSelectListAsync();
}
