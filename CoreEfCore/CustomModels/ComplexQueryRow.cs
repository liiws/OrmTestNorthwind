namespace CoreEfCore.CustomModels
{
	public class ComplexQueryRow
	{
		public short Quantity { get; set; }
		public decimal UnitPrice { get; set; }
		public float Discount { get; set; }
		public string ShipCountry { get; set; }
		public string Country { get; set; }
	}
}
