using AISDproject.Models;
using AISDproject.Repositeries;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AISDproject.Services.Implementation
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _repository;

        public TableService(ITableRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Table>> GetAllTablesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Table> GetTableByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateTableAsync(Table table)
        {
            await _repository.AddAsync(table);
        }

        public async Task UpdateTableAsync(Table table)
        {
            await _repository.UpdateAsync(table);
        }

        public async Task DeleteTableAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<SelectListItem>> GetTablesSelectListAsync()
        {
            var tables = await _repository.GetAllAsync();

            return tables.Select(t => new SelectListItem
            {
                Value = t.TableId.ToString(),
                Text = $"Table {t.TableName}" // Hoặc t.Name nếu có
            });
        }

    }
}
