using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Wrappers;

public class SearchRequest
{
    // Pagination
    private int _pageSize = 10;
    private const int MaxPageSize = 100;

    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    // Search & Sorting
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; } // e.g. "Name", "CreatedOn"
    public bool SortDescending { get; set; } = false;
}

