using DotnetAdo;
using DotnetEfCore;
using DotnetLinq2db;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

namespace TestDotnet
{
	class Program
	{
		static void Main(string[] args)
		{
			var cold = 100;
			var hot = 1000;

			var connectionStringNorthwind = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;
			var linq2dbConfiguration = "NorthWindLinq2db";

			var categoryIds = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
			var supplierIds = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };

			var dotnetAdoTests = new DotnetAdoTests();
			dotnetAdoTests.ConnectionStringNorthwind = connectionStringNorthwind;

			var dotnetLinq2dbTests = new DotnetLinq2dbTests();
			dotnetLinq2dbTests.Linq2dbConfiguration = linq2dbConfiguration;

			var dotnetEfCoreTests = new DotnetEfCoreTests();


			var testsToRun = new List<Tuple<Func<int>, string>>
			{
				new Tuple<Func<int>, string>(() => dotnetAdoTests.SimpleTop10(), "ADO .NET, simple, take 10"),
				new Tuple<Func<int>, string>(() => dotnetAdoTests.SimpleTop500(), "ADO .NET, simple, take 500"),
				new Tuple<Func<int>, string>(() => dotnetAdoTests.ComplexTop10(categoryIds, supplierIds), "ADO .NET, complex, take 10"),
				new Tuple<Func<int>, string>(() => dotnetAdoTests.ComplexTop500(categoryIds, supplierIds), "ADO .NET, complex, take 500"),
				new Tuple<Func<int>, string>(() => dotnetAdoTests.SimpleTop10And10(), "ADO .NET, simple, take 10+10"),

				new Tuple<Func<int>, string>(() => dotnetLinq2dbTests.SimpleTop10(), "LINQ2DB .NET, simple, take 10"),
				new Tuple<Func<int>, string>(() => dotnetLinq2dbTests.SimpleTop500(), "LINQ2DB .NET, simple, take 500"),
				new Tuple<Func<int>, string>(() => dotnetLinq2dbTests.SimpleRawTop10(), "LINQ2DB .NET, simple raw, take 10"),
				new Tuple<Func<int>, string>(() => dotnetLinq2dbTests.SimpleRawTop500(), "LINQ2DB .NET, simple raw, take 500"),
				new Tuple<Func<int>, string>(() => dotnetLinq2dbTests.ComplexTop10(categoryIds, supplierIds), "LINQ2DB .NET, complex, take 10"),
				new Tuple<Func<int>, string>(() => dotnetLinq2dbTests.ComplexTop500(categoryIds, supplierIds), "LINQ2DB .NET, complex, take 500"),
				new Tuple<Func<int>, string>(() => dotnetLinq2dbTests.ComplexRawTop10(categoryIds, supplierIds), "LINQ2DB .NET, complex raw, take 10"),
				new Tuple<Func<int>, string>(() => dotnetLinq2dbTests.ComplexRawTop500(categoryIds, supplierIds), "LINQ2DB .NET, complex raw, take 500"),
				new Tuple<Func<int>, string>(() => dotnetLinq2dbTests.SimpleTop10And10(), "LINQ2DB .NET, simple, take 10+10"),
				new Tuple<Func<int>, string>(() => dotnetLinq2dbTests.SimpleRawTop10And10(), "LINQ2DB .NET, simple raw, take 10+10"),

				new Tuple<Func<int>, string>(() => dotnetEfCoreTests.SimpleTop10(), "EFCore .NET, simple, take 10"),
				new Tuple<Func<int>, string>(() => dotnetEfCoreTests.SimpleTop500(), "EFCore .NET, simple, take 500"),
				new Tuple<Func<int>, string>(() => dotnetEfCoreTests.SimpleRawTop10(), "EFCore .NET, simple raw, take 10"),
				new Tuple<Func<int>, string>(() => dotnetEfCoreTests.SimpleRawTop500(), "EFCore .NET, simple raw, take 500"),
				new Tuple<Func<int>, string>(() => dotnetEfCoreTests.ComplexTop10(categoryIds, supplierIds), "EFCore .NET, complex, take 10"),
				new Tuple<Func<int>, string>(() => dotnetEfCoreTests.ComplexTop500(categoryIds, supplierIds), "EFCore .NET, complex, take 500"),
				new Tuple<Func<int>, string>(() => dotnetEfCoreTests.ComplexRawTop10(categoryIds, supplierIds), "EFCore .NET, complex raw, take 10"),
				new Tuple<Func<int>, string>(() => dotnetEfCoreTests.ComplexRawTop500(categoryIds, supplierIds), "EFCore .NET, complex raw, take 500"),
				new Tuple<Func<int>, string>(() => dotnetEfCoreTests.SimpleTop10And10(), "EFCore .NET, simple, take 10+10"),
				new Tuple<Func<int>, string>(() => dotnetEfCoreTests.SimpleRawTop10And10(), "EFCore .NET, simple raw, take 10+10"),
			};



			Console.WriteLine("Tests configuration:");
			Console.WriteLine("cold = " + cold);
			Console.WriteLine("hot = " + hot);
			Console.WriteLine("---");

			foreach (var testItem in testsToRun)
			{
				var result = Test(testItem.Item1, cold, hot);
				Console.WriteLine("{0,-50} = {1,8} ms/request", testItem.Item2, 1f * result / hot);
			}

			Console.WriteLine("\r\nEND\r\n");
			Console.ReadKey();
		}

		static long Test(Func<int> f, int cold, int hot)
		{
			var isCold = true;
			while (true)
			{
				var n = isCold ? cold : hot;
				var tmpI = 0;
				var sw = new Stopwatch();
				sw.Start();
				for (var i = 0; i < n; i++)
				{
					tmpI += f();
				}
				var elapsedMs = sw.ElapsedMilliseconds;

				if (isCold)
				{
					isCold = false;
				}
				else
				{
					return elapsedMs;
				}
			}
		}
	}
}
