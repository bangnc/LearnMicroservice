using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.DTOs.Page
{
    public abstract class PageRequest
    {
        private const int MaxPageSize = 100;

        public int PageNumber { get; init; } = 1;

        private int _pageSize = 20;

        public int PageSize
        {
            get => _pageSize;
            init => _pageSize = Math.Clamp(value, 1, MaxPageSize);
        }

        public string? Search { get; init; }

        public string? SortBy { get; init; }

        public bool Desc { get; init; }
    }
}
