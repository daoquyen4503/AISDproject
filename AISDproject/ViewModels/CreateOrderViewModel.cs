using AISDproject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AISDproject.ViewModels
{
    public class CreateOrderViewModel
    {
        public List<MenuItem> MenuItems { get; set; }
        public Dictionary<int, int> Quantities { get; set; } = new Dictionary<int, int>();
        public int TableId { get; set; }
        public int StaffId { get; set; }

        public List<SelectListItem> TableList { get; set; } = new();
        public List<SelectListItem> StaffList { get; set; } = new();
    }
}
