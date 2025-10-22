using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DTOs.MenuCategoryDTOs
{
    public class CreateMenuCategoryDTO
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
