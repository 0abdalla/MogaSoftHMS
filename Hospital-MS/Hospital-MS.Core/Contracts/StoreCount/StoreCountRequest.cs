using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.StoreCount;
public class StoreCountRequest
{
    public int StoreId { get; set; } 
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; } 
}
