﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Disbursement;
public class DisbursementItemReq
{
    public int ItemId { get; set; }
    public int Quantity { get; set; }
}
