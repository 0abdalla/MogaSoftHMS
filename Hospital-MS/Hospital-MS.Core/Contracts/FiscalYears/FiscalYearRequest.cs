﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.FiscalYears;
public class FiscalYearRequest
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}
