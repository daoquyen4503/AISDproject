using AISDproject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AISDproject.Services
{
    public interface IStaffService
    {
        Task<IEnumerable<Staff>> GetAllStaffAsync();
        Task<Staff> GetStaffByIdAsync(int id);
        Task CreateStaffAsync(Staff staff);
        Task UpdateStaffAsync(Staff staff);
        Task DeleteStaffAsync(int id);
        Task<IEnumerable<SelectListItem>> GetStaffsSelectListAsync();
    }
}
