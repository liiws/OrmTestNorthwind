using System;
using System.Collections.Generic;

namespace CoreEfCore.Models
{
    public partial class OrderSubtotals
    {
        public int OrderId { get; set; }
        public decimal? Subtotal { get; set; }
    }
}
