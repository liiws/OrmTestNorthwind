using LinqToDB.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace CoreLinq2db
{
	public class Linq2DbConfig
	{
		public class ConnectionStringSettings : IConnectionStringSettings
		{
			public string ConnectionString { get; set; }
			public string Name { get; set; }
			public string ProviderName { get; set; }
			public bool IsGlobal => false;
		}

		public class Linq2DbSettings : ILinqToDBSettings
		{
			private string _connectionString;

			public Linq2DbSettings(string connectionString)
			{
				_connectionString = connectionString;
			}

			public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();

			public string DefaultConfiguration => "SqlServer";
			public string DefaultDataProvider => "SqlServer";

			public IEnumerable<IConnectionStringSettings> ConnectionStrings
			{
				get
				{
					yield return
						new ConnectionStringSettings
						{
							Name = "SqlServer",
							ProviderName = "SqlServer",
							ConnectionString = _connectionString,
						};
				}
			}
		}
	}
}
