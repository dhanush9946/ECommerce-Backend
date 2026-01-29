using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Product
{
    public class AdminProductQueryDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }
        public bool? IsActive { get; set; }
        public string? Category { get; set; }
        public string? Brand { get; set; }

        public string? SortBy { get; set; }      // price, createdAt, stock
        public string? SortOrder { get; set; }   // asc | desc
    }
}
