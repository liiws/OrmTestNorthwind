using System;

namespace CoreEfCore.CustomModels
{
	public class SimpleQueryRow
	{
		public int OrderId { get; set; }
		public DateTime? OrderDate { get; set; }
		public string Country { get; set; }
		public string CompanyName { get; set; }
	}
}
