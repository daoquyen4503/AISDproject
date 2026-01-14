using AISDproject.Models;
using AISDproject.Repositeries;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AISDproject.Services.Implementation
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepository;

        public StaffService(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }

        public async Task<IEnumerable<Staff>> GetAllStaffAsync()
        {
            return await _staffRepository.GetAllAsync();
        }

        public async Task<Staff> GetStaffByIdAsync(int id)
        {
            return await _staffRepository.GetByIdAsync(id);
        }

        public async Task CreateStaffAsync(Staff staff)
        {
            await _staffRepository.AddAsync(staff);
        }

        public async Task UpdateStaffAsync(Staff staff)
        {
            await _staffRepository.UpdateAsync(staff);
        }

        public async Task DeleteStaffAsync(int id)
        {
            await _staffRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<SelectListItem>> GetStaffsSelectListAsync()
        {
            var staffs = await _staffRepository.GetAllAsync();

            return staffs.Select(s => new SelectListItem
            {
                Value = s.StaffId.ToString(),
                Text = s.FullName
            });
        }

    }
}
