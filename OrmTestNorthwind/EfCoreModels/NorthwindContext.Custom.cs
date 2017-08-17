using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OrmTestNorthwind.EfCoreModels
{
	public partial class NorthwindContext : DbContext
	{
		//public NorthwindContext()
		//{
		//}

		public NorthwindContext(DbContextOptions<NorthwindContext> options) : base(options)
		{
		}
	}
}
