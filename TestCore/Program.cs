using CoreAdo;
using CoreLinq2db;
using CoreEfCore;
using LinqToDB.Data;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace TestCore
{
	class Program
	{
		static void Main(string[] args)
		{
			var cold = 100;
			var hot = 1000;

			var connectionStringNorthwind = "Data Source=localhost;Initial Catalog=Northwind;Integrated Security=True";

			var categoryIds = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
			var supplierIds = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };

			var coreAdoTests = new CoreAdoTests();
			coreAdoTests.ConnectionStringNorthwind = connectionStringNorthwind;

			var coreLinq2dbTests = new CoreLinq2dbTests();
			DataConnection.DefaultSettings = new Linq2DbConfig.Linq2DbSettings(connectionStringNorthwind);

			var coreEfCoreTests = new CoreEfCoreTests();



			var testsToRun = new List<Tuple<Func<int>, string>>
			{
				new Tuple<Func<int>, string>(() => coreAdoTests.SimpleTop10(), "ADO Core, simple, take 10"),
				new Tuple<Func<int>, string>(() => coreAdoTests.SimpleTop500(), "ADO Core, simple, take 500"),
				new Tuple<Func<int>, string>(() => coreAdoTests.ComplexTop10(categoryIds, supplierIds), "ADO Core, complex, take 10"),
				new Tuple<Func<int>, string>(() => coreAdoTests.ComplexTop500(categoryIds, supplierIds), "ADO Core, complex, take 500"),
				new Tuple<Func<int>, string>(() => coreAdoTests.SimpleTop10And10(), "ADO Core, simple, take 10+10"),

				new Tuple<Func<int>, string>(() => coreLinq2dbTests.SimpleTop10(), "LINQ2DB Core, simple, take 10"),
				new Tuple<Func<int>, string>(() => coreLinq2dbTests.SimpleTop500(), "LINQ2DB Core, simple, take 500"),
				new Tuple<Func<int>, string>(() => coreLinq2dbTests.SimpleRawTop10(), "LINQ2DB Core, simple raw, take 10"),
				new Tuple<Func<int>, string>(() => coreLinq2dbTests.SimpleRawTop500(), "LINQ2DB Core, simple raw, take 500"),
				new Tuple<Func<int>, string>(() => coreLinq2dbTests.ComplexTop10(categoryIds, supplierIds), "LINQ2DB Core, complex, take 10"),
				new Tuple<Func<int>, string>(() => coreLinq2dbTests.ComplexTop500(categoryIds, supplierIds), "LINQ2DB Core, complex, take 500"),
				new Tuple<Func<int>, string>(() => coreLinq2dbTests.ComplexRawTop10(categoryIds, supplierIds), "LINQ2DB Core, complex raw, take 10"),
				new Tuple<Func<int>, string>(() => coreLinq2dbTests.ComplexRawTop500(categoryIds, supplierIds), "LINQ2DB Core, complex raw, take 500"),
				new Tuple<Func<int>, string>(() => coreLinq2dbTests.SimpleTop10And10(), "LINQ2DB Core, simple, take 10+10"),
				new Tuple<Func<int>, string>(() => coreLinq2dbTests.SimpleRawTop10And10(), "LINQ2DB Core, simple raw, take 10+10"),

				new Tuple<Func<int>, string>(() => coreEfCoreTests.SimpleTop10(), "EFCore Core, simple, take 10"),
				new Tuple<Func<int>, string>(() => coreEfCoreTests.SimpleTop500(), "EFCore Core, simple, take 500"),
				new Tuple<Func<int>, string>(() => coreEfCoreTests.SimpleRawTop10(), "EFCore Core, simple raw, take 10"),
				new Tuple<Func<int>, string>(() => coreEfCoreTests.SimpleRawTop500(), "EFCore Core, simple raw, take 500"),
				new Tuple<Func<int>, string>(() => coreEfCoreTests.ComplexTop10(categoryIds, supplierIds), "EFCore Core, complex, take 10"),
				new Tuple<Func<int>, string>(() => coreEfCoreTests.ComplexTop500(categoryIds, supplierIds), "EFCore Core, complex, take 500"),
				new Tuple<Func<int>, string>(() => coreEfCoreTests.ComplexRawTop10(categoryIds, supplierIds), "EFCore Core, complex raw, take 10"),
				new Tuple<Func<int>, string>(() => coreEfCoreTests.ComplexRawTop500(categoryIds, supplierIds), "EFCore Core, complex raw, take 500"),
				new Tuple<Func<int>, string>(() => coreEfCoreTests.SimpleTop10And10(), "EFCore Core, simple, take 10+10"),
				new Tuple<Func<int>, string>(() => coreEfCoreTests.SimpleRawTop10And10(), "EFCore Core, simple raw, take 10+10"),
			};



			Console.WriteLine("Tests configuration:");
			Console.WriteLine("cold = " + cold);
			Console.WriteLine("hot = " + hot);
			Console.WriteLine("---");

			foreach (var testItem in testsToRun)
			{
				var result = Test(testItem.Item1, cold, hot);
				Console.WriteLine("{0,-50} = {1,8} ms/request", testItem.Item2, 1f*result/hot);
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
