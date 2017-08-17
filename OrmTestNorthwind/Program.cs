using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Diagnostics;
using LinqToDB.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace OrmTestNorthwind
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



			// EF Core Settings:
			// 1) Pooling - doesn't affect performance.
			// 2) Disabled tracking - doesn't affect performance of selects.

			var serviceProvider =
				new ServiceCollection()
				.AddEntityFrameworkSqlServer()
				.AddDbContext<EfCoreModels.NorthwindContext>(options => options.UseSqlServer(connectionStringNorthwind))
				.BuildServiceProvider();

			var efOptions = new DbContextOptionsBuilder<EfCoreModels.NorthwindContext>()
				.UseInternalServiceProvider(serviceProvider)
				//.UseSqlServer(connectionStringNorthwind)
				.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
				.Options;




			// SIMPLE QUERY


			Func <int> simpleEf6CodeFirstTop10 = () =>
			{
				using (var ctx = new Ef6CodeFirst.Ef6CodeFirst())
				{
					var list =
						(
							from o in ctx.Orders
							join c in ctx.Customers on o.CustomerID equals c.CustomerID
							select new { o.OrderID, o.OrderDate, c.Country, c.CompanyName }
						).Take(10).ToList();
					return list.Count;
				}
			};

			Func<int> simpleEf6CodeFirstTop500 = () =>
			{
				using (var ctx = new Ef6CodeFirst.Ef6CodeFirst())
				{
					var list =
						(
							from o in ctx.Orders
							join c in ctx.Customers on o.CustomerID equals c.CustomerID
							select new { o.OrderID, o.OrderDate, c.Country, c.CompanyName }
							).Take(500).ToList();
					return list.Count;
				}
			};

			//			Func<int> simpleEf6CodeFirstRawTop10 = () =>
			//			{
			//				using (var ctx = new Ef6CodeFirst.Ef6CodeFirst())
			//				{
			//					var sql = @"
			//SELECT TOP 10 O.OrderID, O.OrderDate, C.Country, C.CompanyName
			//FROM Orders O
			//JOIN Customers C ON O.CustomerID = C.CustomerID
			//							";
			//					var list = ctx.Database.SqlQuery<SimpleQueryRow>(sql).ToList();
			//					return list.Count;
			//				}
			//			};

			//			Func<int> simpleEf6CodeFirstRawTop500 = () =>
			//			{
			//				using (var ctx = new Ef6CodeFirst.Ef6CodeFirst())
			//				{
			//					var sql = @"
			//SELECT TOP 500 O.OrderID, O.OrderDate, C.Country, C.CompanyName
			//FROM Orders O
			//JOIN Customers C ON O.CustomerID = C.CustomerID
			//							";
			//					var list = ctx.Database.SqlQuery<SimpleQueryRow>(sql).ToList();
			//					return list.Count;
			//				}
			//			};

			Func<int> simpleAdoNetTop10 = () =>
			{
				int count = 0;
				using (var con = new SqlConnection(connectionStringNorthwind))
				{
					con.Open();
					var sql = @"
SELECT TOP 10 O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
					using (var cmd = new SqlCommand(sql, con))
					{
						using (var reader = cmd.ExecuteReader())
						{
							if (reader.HasRows)
							{
								var list = new List<SimpleQueryRow>();
								while (reader.Read())
								{
									list.Add(new SimpleQueryRow
									{
										OrderId = reader.GetInt32(0),
										OrderDate = reader.IsDBNull(1) ? null : (DateTime?)reader.GetDateTime(1),
										Country = reader.IsDBNull(2) ? null : reader.GetString(2),
										CompanyName = reader.IsDBNull(3) ? null : reader.GetString(3),
									});
								}
								count = list.Count;
							}
						}
					}
				}
				return count;
			};

			Func<int> simpleAdoNetTop500 = () =>
			{
				int count = 0;
				using (var con = new SqlConnection(connectionStringNorthwind))
				{
					con.Open();
					var sql = @"
SELECT TOP 500 O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
					using (var cmd = new SqlCommand(sql, con))
					{
						using (var reader = cmd.ExecuteReader())
						{
							if (reader.HasRows)
							{
								var list = new List<SimpleQueryRow>();
								while (reader.Read())
								{
									list.Add(new SimpleQueryRow
									{
										OrderId = reader.GetInt32(0),
										OrderDate = reader.IsDBNull(1) ? null : (DateTime?)reader.GetDateTime(1),
										Country = reader.IsDBNull(2) ? null : reader.GetString(2),
										CompanyName = reader.IsDBNull(3) ? null : reader.GetString(3),
									});
								}
								count = list.Count;
							}
						}
					}
				}
				return count;
			};

			Func<int> simpleLinq2DbTop10 = () =>
			{
				using (var db = new DataModels.NorthwindDB(linq2dbConfiguration))
				{
					var list =
						(
							from o in db.Orders
							join c in db.Customers on o.CustomerID equals c.CustomerID
							select new { o.OrderID, o.OrderDate, c.Country, c.CompanyName }
						).Take(10).ToList();
					return list.Count;
				}
			};

			Func<int> simpleLinq2DbTop500 = () =>
			{
				using (var db = new DataModels.NorthwindDB(linq2dbConfiguration))
				{
					var list =
						(
							from o in db.Orders
							join c in db.Customers on o.CustomerID equals c.CustomerID
							select new { o.OrderID, o.OrderDate, c.Country, c.CompanyName }
						).Take(500).ToList();
					return list.Count;
				}
			};

			Func<int> simpleLinq2DbRawTop10 = () =>
			{
				using (var db = new DataModels.NorthwindDB(linq2dbConfiguration))
				{
					var sql = @"
SELECT TOP 10 O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
					var list = db.Query<SimpleQueryRow>(sql).ToList();
					return list.Count;
				}
			};

			Func<int> simpleLinq2DbRawTop500 = () =>
			{
				using (var db = new DataModels.NorthwindDB(linq2dbConfiguration))
				{
					var sql = @"
SELECT TOP 500 O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
					var list = db.Query<SimpleQueryRow>(sql).ToList();
					return list.Count;
				}
			};

			Func<int> simpleEfCoreTop10 = () =>
			{
				using (var db = new EfCoreModels.NorthwindContext(efOptions))
				{
					var list =
						(
							from o in db.Orders
							join c in db.Customers on o.CustomerId equals c.CustomerId
							select new { o.OrderId, o.OrderDate, c.Country, c.CompanyName }
						).Take(10).ToList();
					return list.Count;
				}
			};

			Func<int> simpleEfCoreTop500 = () =>
			{
				using (var db = new EfCoreModels.NorthwindContext())
				{
					var list =
						(
							from o in db.Orders
							join c in db.Customers on o.CustomerId equals c.CustomerId
							select new { o.OrderId, o.OrderDate, c.Country, c.CompanyName }
						).Take(500).ToList();
					return list.Count;
				}
			};

			Func<int> simpleEfCoreRawTop10 = () =>
			{
				using (var db = new EfCoreModels.NorthwindContext())
				{
					var sql = @"
SELECT TOP 10 O.*--O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
					var list = db.Orders.FromSql(sql).ToList();
					return list.Count;
				}
			};

			Func<int> simpleEfCoreRawTop500 = () =>
			{
				using (var db = new EfCoreModels.NorthwindContext())
				{
					var sql = @"
SELECT TOP 500 O.*--O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
					var list = db.Orders.FromSql(sql).ToList();
					return list.Count;
				}
			};

			Func<EfCoreModels.NorthwindContext, IEnumerable<SimpleQueryRow>> simpleEfCoreCompiledTop10CompiledQuery =
				EF.CompileQuery((EfCoreModels.NorthwindContext db) =>
					(
						from o in db.Orders
						join c in db.Customers on o.CustomerId equals c.CustomerId
						select new SimpleQueryRow { OrderId = o.OrderId, OrderDate = o.OrderDate, Country = c.Country, CompanyName = c.CompanyName }
					).Take(10)
				);
			Func<int> simpleEfCoreCompiledTop10 = () =>
			{
				using (var db = new EfCoreModels.NorthwindContext())
				{
					var list = simpleEfCoreCompiledTop10CompiledQuery(db).ToList();
					return list.Count;
				}
			};

			Func<EfCoreModels.NorthwindContext, IEnumerable<SimpleQueryRow>> simpleEfCoreCompiledTop500CompiledQuery =
				EF.CompileQuery((EfCoreModels.NorthwindContext db) =>
					(
						from o in db.Orders
						join c in db.Customers on o.CustomerId equals c.CustomerId
						select new SimpleQueryRow { OrderId = o.OrderId, OrderDate = o.OrderDate, Country = c.Country, CompanyName = c.CompanyName }
					).Take(500)
				);
			Func<int> simpleEfCoreCompiledTop500 = () =>
			{
				using (var db = new EfCoreModels.NorthwindContext())
				{
					var list = simpleEfCoreCompiledTop500CompiledQuery(db).ToList();
					return list.Count;
				}
			};




			// COMPLEX QUERY



			Func<int> complexEf6CodeFirstTop10 = () =>
			{
				using (var ctx = new Ef6CodeFirst.Ef6CodeFirst())
				{
					var list =
						(
							from o in ctx.Orders
							join od in ctx.Order_Details on o.OrderID equals od.OrderID
							join p in ctx.Products on od.ProductID equals p.ProductID
							join cat in ctx.Categories on p.CategoryID equals cat.CategoryID
							join s in ctx.Suppliers on p.SupplierID equals s.SupplierID
							where categoryIds.Contains(cat.CategoryID)
								&& supplierIds.Contains(s.SupplierID)
							orderby od.Discount descending
							select new { od.Quantity, od.UnitPrice, od.Discount, o.ShipCountry, s.Country }
						).Take(10).ToList();
					return list.Count;
				}
			};

			Func<int> complexEf6CodeFirstTop500 = () =>
			{
				using (var ctx = new Ef6CodeFirst.Ef6CodeFirst())
				{
					var list =
						(
							from o in ctx.Orders
							join od in ctx.Order_Details on o.OrderID equals od.OrderID
							join p in ctx.Products on od.ProductID equals p.ProductID
							join cat in ctx.Categories on p.CategoryID equals cat.CategoryID
							join s in ctx.Suppliers on p.SupplierID equals s.SupplierID
							where categoryIds.Contains(cat.CategoryID)
								&& supplierIds.Contains(s.SupplierID)
							orderby od.Discount descending
							select new { od.Quantity, od.UnitPrice, od.Discount, o.ShipCountry, s.Country }
						).Take(500).ToList();
					return list.Count;
				}
			};

			//			Func<int> complexEf6CodeFirstRawTop10 = () =>
			//			{
			//				using (var ctx = new Ef6CodeFirst.Ef6CodeFirst())
			//				{
			//					var sql = @"
			//SELECT TOP 10 OD.Quantity, OD.UnitPrice, OD.Discount, O.ShipCountry, S.Country
			//FROM Orders O
			//JOIN [Order Details] OD ON O.OrderID = OD.OrderID
			//JOIN Products P ON OD.ProductID = P.ProductID
			//JOIN Categories Cat ON P.CategoryID = Cat.CategoryID
			//JOIN Suppliers S ON P.SupplierID = S.SupplierID
			//WHERE
			//	Cat.CategoryID IN (@categoryIds)
			//	AND S.SupplierID IN (@supplierIds)
			//ORDER BY OD.Discount DESC
			//						".Replace("@categoryIds", string.Join(",", categoryIds))
			//						 .Replace("@supplierIds", string.Join(",", supplierIds));
			//					var list = ctx.Database.SqlQuery<ComplexQueryRow>(sql).ToList();
			//					return list.Count;
			//				}
			//			};

			//			Func<int> complexEf6CodeFirstRawTop500 = () =>
			//			{
			//				using (var ctx = new Ef6CodeFirst.Ef6CodeFirst())
			//				{
			//					var sql = @"
			//SELECT TOP 500 OD.Quantity, OD.UnitPrice, OD.Discount, O.ShipCountry, S.Country
			//FROM Orders O
			//JOIN [Order Details] OD ON O.OrderID = OD.OrderID
			//JOIN Products P ON OD.ProductID = P.ProductID
			//JOIN Categories Cat ON P.CategoryID = Cat.CategoryID
			//JOIN Suppliers S ON P.SupplierID = S.SupplierID
			//WHERE
			//	Cat.CategoryID IN (@categoryIds)
			//	AND S.SupplierID IN (@supplierIds)
			//ORDER BY OD.Discount DESC
			//						".Replace("@categoryIds", string.Join(",", categoryIds))
			//						 .Replace("@supplierIds", string.Join(",", supplierIds));
			//					var list = ctx.Database.SqlQuery<ComplexQueryRow>(sql).ToList();
			//					return list.Count;
			//				}
			//			};

			Func<int> complexAdoNetTop10 = () =>
			{
				int count = 0;
				using (var con = new SqlConnection(connectionStringNorthwind))
				{
					con.Open();
					var sql = @"
SELECT TOP 10 OD.Quantity, OD.UnitPrice, OD.Discount, O.ShipCountry, S.Country
FROM Orders O
JOIN [Order Details] OD ON O.OrderID = OD.OrderID
JOIN Products P ON OD.ProductID = P.ProductID
JOIN Categories Cat ON P.CategoryID = Cat.CategoryID
JOIN Suppliers S ON P.SupplierID = S.SupplierID
WHERE
	Cat.CategoryID IN (@categoryIds)
	AND S.SupplierID IN (@supplierIds)
ORDER BY OD.Discount DESC
							".Replace("@categoryIds", string.Join(",", categoryIds))
						 .Replace("@supplierIds", string.Join(",", supplierIds));
					using (var cmd = new SqlCommand(sql, con))
					{
						using (var reader = cmd.ExecuteReader())
						{
							if (reader.HasRows)
							{
								var list = new List<ComplexQueryRow>();
								while (reader.Read())
								{
									list.Add(new ComplexQueryRow
									{
										Quantity = reader.GetInt16(0),
										UnitPrice = reader.GetDecimal(1),
										Discount = reader.GetFloat(2),
										ShipCountry = reader.IsDBNull(1) ? null : reader.GetString(3),
										Country = reader.IsDBNull(1) ? null : reader.GetString(4),
									});
								}
								count = list.Count;
							}
						}
					}
				}
				return count;
			};

			Func<int> complexAdoNetTop500 = () =>
			{
				int count = 0;
				using (var con = new SqlConnection(connectionStringNorthwind))
				{
					con.Open();
					var sql = @"
SELECT TOP 500 OD.Quantity, OD.UnitPrice, OD.Discount, O.ShipCountry, S.Country
FROM Orders O
JOIN [Order Details] OD ON O.OrderID = OD.OrderID
JOIN Products P ON OD.ProductID = P.ProductID
JOIN Categories Cat ON P.CategoryID = Cat.CategoryID
JOIN Suppliers S ON P.SupplierID = S.SupplierID
WHERE
	Cat.CategoryID IN (@categoryIds)
	AND S.SupplierID IN (@supplierIds)
ORDER BY OD.Discount DESC
							".Replace("@categoryIds", string.Join(",", categoryIds))
						 .Replace("@supplierIds", string.Join(",", supplierIds));
					using (var cmd = new SqlCommand(sql, con))
					{
						using (var reader = cmd.ExecuteReader())
						{
							if (reader.HasRows)
							{
								var list = new List<ComplexQueryRow>();
								while (reader.Read())
								{
									list.Add(new ComplexQueryRow
									{
										Quantity = reader.GetInt16(0),
										UnitPrice = reader.GetDecimal(1),
										Discount = reader.GetFloat(2),
										ShipCountry = reader.IsDBNull(1) ? null : reader.GetString(3),
										Country = reader.IsDBNull(1) ? null : reader.GetString(4),
									});
								}
								count = list.Count;
							}
						}
					}
				}
				return count;
			};

			Func<int> complexLinq2DbTop10 = () =>
			{
				using (var db = new DataModels.NorthwindDB(linq2dbConfiguration))
				{
					var list =
						(
							from o in db.Orders
							join od in db.OrderDetails on o.OrderID equals od.OrderID
							join p in db.Products on od.ProductID equals p.ProductID
							join cat in db.Categories on p.CategoryID equals cat.CategoryID
							join s in db.Suppliers on p.SupplierID equals s.SupplierID
							where categoryIds.Contains(cat.CategoryID)
								&& supplierIds.Contains(s.SupplierID)
							orderby od.Discount descending
							select new { od.Quantity, od.UnitPrice, od.Discount, o.ShipCountry, s.Country }
						).Take(10).ToList();
					return list.Count;
				}
			};

			Func<int> complexLinq2DbTop500 = () =>
			{
				using (var db = new DataModels.NorthwindDB(linq2dbConfiguration))
				{
					var list =
						(
							from o in db.Orders
							join od in db.OrderDetails on o.OrderID equals od.OrderID
							join p in db.Products on od.ProductID equals p.ProductID
							join cat in db.Categories on p.CategoryID equals cat.CategoryID
							join s in db.Suppliers on p.SupplierID equals s.SupplierID
							where categoryIds.Contains(cat.CategoryID)
								&& supplierIds.Contains(s.SupplierID)
							orderby od.Discount descending
							select new { od.Quantity, od.UnitPrice, od.Discount, o.ShipCountry, s.Country }
						).Take(500).ToList();
					return list.Count;
				}
			};

			Func<int> complexLinq2DbRawTop10 = () =>
			{
				using (var db = new DataModels.NorthwindDB(linq2dbConfiguration))
				{
					var sql = @"
SELECT TOP 10 OD.Quantity, OD.UnitPrice, OD.Discount, O.ShipCountry, S.Country
FROM Orders O
JOIN [Order Details] OD ON O.OrderID = OD.OrderID
JOIN Products P ON OD.ProductID = P.ProductID
JOIN Categories Cat ON P.CategoryID = Cat.CategoryID
JOIN Suppliers S ON P.SupplierID = S.SupplierID
WHERE
	Cat.CategoryID IN (@categoryIds)
	AND S.SupplierID IN (@supplierIds)
ORDER BY OD.Discount DESC
						".Replace("@categoryIds", string.Join(",", categoryIds))
						 .Replace("@supplierIds", string.Join(",", supplierIds));
					var list = db.Query<ComplexQueryRow>(sql).ToList();
					return list.Count;
				}
			};

			Func<int> complexLinq2DbRawTop500 = () =>
			{
				using (var db = new DataModels.NorthwindDB(linq2dbConfiguration))
				{
					var sql = @"
SELECT TOP 500 OD.Quantity, OD.UnitPrice, OD.Discount, O.ShipCountry, S.Country
FROM Orders O
JOIN [Order Details] OD ON O.OrderID = OD.OrderID
JOIN Products P ON OD.ProductID = P.ProductID
JOIN Categories Cat ON P.CategoryID = Cat.CategoryID
JOIN Suppliers S ON P.SupplierID = S.SupplierID
WHERE
	Cat.CategoryID IN (@categoryIds)
	AND S.SupplierID IN (@supplierIds)
ORDER BY OD.Discount DESC
						".Replace("@categoryIds", string.Join(",", categoryIds))
						 .Replace("@supplierIds", string.Join(",", supplierIds));
					var list = db.Query<ComplexQueryRow>(sql).ToList();
					return list.Count;
				}
			};


			Func<int> complexEfCoreTop10 = () =>
			{
				using (var db = new EfCoreModels.NorthwindContext())
				{
					var list =
						(
							from o in db.Orders
							join od in db.OrderDetails on o.OrderId equals od.OrderId
							join p in db.Products on od.ProductId equals p.ProductId
							join cat in db.Categories on p.CategoryId equals cat.CategoryId
							join s in db.Suppliers on p.SupplierId equals s.SupplierId
							where categoryIds.Contains(cat.CategoryId)
								&& supplierIds.Contains(s.SupplierId)
							orderby od.Discount descending
							select new { od.Quantity, od.UnitPrice, od.Discount, o.ShipCountry, s.Country }
						).Take(10).ToList();
					return list.Count;
				}
			};

			Func<int> complexEfCoreTop500 = () =>
			{
				using (var db = new EfCoreModels.NorthwindContext())
				{
					var list =
						(
							from o in db.Orders
							join od in db.OrderDetails on o.OrderId equals od.OrderId
							join p in db.Products on od.ProductId equals p.ProductId
							join cat in db.Categories on p.CategoryId equals cat.CategoryId
							join s in db.Suppliers on p.SupplierId equals s.SupplierId
							where categoryIds.Contains(cat.CategoryId)
								&& supplierIds.Contains(s.SupplierId)
							orderby od.Discount descending
							select new { od.Quantity, od.UnitPrice, od.Discount, o.ShipCountry, s.Country }
						).Take(500).ToList();
					return list.Count;
				}
			};





			// SIMPLE 10 + 10 QUERY (TO CALCULATE CONTEXT INITIALIZATION TIME)



			Func<int> simpleEf6CodeFirstTop10And10 = () =>
			{
				using (var ctx = new Ef6CodeFirst.Ef6CodeFirst())
				{
					var list =
						(
							from o in ctx.Orders
							join c in ctx.Customers on o.CustomerID equals c.CustomerID
							select new { o.OrderID, o.OrderDate, c.Country, c.CompanyName }
						).Take(10).ToList();
					var list2 =
						(
							from o in ctx.Orders
							join c in ctx.Customers on o.CustomerID equals c.CustomerID
							select new { o.OrderID, o.OrderDate, c.Country, c.CompanyName }
						).Take(10).ToList();
					return list.Count + list2.Count;
				}
			};

			//			Func<int> simpleEf6CodeFirstRawTop10And10 = () =>
			//			{
			//				using (var ctx = new Ef6CodeFirst.Ef6CodeFirst())
			//				{
			//					var sql = @"
			//SELECT TOP 10 O.OrderID, O.OrderDate, C.Country, C.CompanyName
			//FROM Orders O
			//JOIN Customers C ON O.CustomerID = C.CustomerID
			//							";
			//					var list = ctx.Database.SqlQuery<SimpleQueryRow>(sql).ToList();

			//					sql = @"
			//SELECT TOP 10 O.OrderID, O.OrderDate, C.Country, C.CompanyName
			//FROM Orders O
			//JOIN Customers C ON O.CustomerID = C.CustomerID
			//							";
			//					var list2 = ctx.Database.SqlQuery<SimpleQueryRow>(sql).ToList();

			//					return list.Count + list2.Count;
			//				}
			//			};

			Func<int> simpleAdoNetTop10And10 = () =>
			{
				int count = 0;
				using (var con = new SqlConnection(connectionStringNorthwind))
				{
					con.Open();
					var sql = @"
SELECT TOP 10 O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
					using (var cmd = new SqlCommand(sql, con))
					{
						using (var reader = cmd.ExecuteReader())
						{
							if (reader.HasRows)
							{
								var list = new List<SimpleQueryRow>();
								while (reader.Read())
								{
									list.Add(new SimpleQueryRow
									{
										OrderId = reader.GetInt32(0),
										OrderDate = reader.IsDBNull(1) ? null : (DateTime?)reader.GetDateTime(1),
										Country = reader.IsDBNull(2) ? null : reader.GetString(2),
										CompanyName = reader.IsDBNull(3) ? null : reader.GetString(3),
									});
								}
								count = list.Count;
							}
						}
					}

					sql = @"
SELECT TOP 10 O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
					using (var cmd = new SqlCommand(sql, con))
					{
						using (var reader = cmd.ExecuteReader())
						{
							if (reader.HasRows)
							{
								var list = new List<SimpleQueryRow>();
								while (reader.Read())
								{
									list.Add(new SimpleQueryRow
									{
										OrderId = reader.GetInt32(0),
										OrderDate = reader.IsDBNull(1) ? null : (DateTime?)reader.GetDateTime(1),
										Country = reader.IsDBNull(2) ? null : reader.GetString(2),
										CompanyName = reader.IsDBNull(3) ? null : reader.GetString(3),
									});
								}
								count += list.Count;
							}
						}
					}
				}
				return count;
			};

			Func<int> simpleLinq2DbTop10And10 = () =>
			{
				using (var db = new DataModels.NorthwindDB(linq2dbConfiguration))
				{
					var list =
						(
							from o in db.Orders
							join c in db.Customers on o.CustomerID equals c.CustomerID
							select new { o.OrderID, o.OrderDate, c.Country, c.CompanyName }
						).Take(10).ToList();
					var list2 =
						(
							from o in db.Orders
							join c in db.Customers on o.CustomerID equals c.CustomerID
							select new { o.OrderID, o.OrderDate, c.Country, c.CompanyName }
						).Take(10).ToList();
					return list.Count + list2.Count;
				}
			};

			Func<int> simpleLinq2DbRawTop10And10 = () =>
			{
				using (var db = new DataModels.NorthwindDB(linq2dbConfiguration))
				{
					var sql = @"
SELECT TOP 10 O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
					var list = db.Query<SimpleQueryRow>(sql).ToList();

					sql = @"
SELECT TOP 10 O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
					var list2 = db.Query<SimpleQueryRow>(sql).ToList();

					return list.Count + list2.Count;
				}
			};


			Func<int> simpleEfCoreTop10And10 = () =>
			{
				using (var db = new EfCoreModels.NorthwindContext())
				{
					var list =
						(
							from o in db.Orders
							join c in db.Customers on o.CustomerId equals c.CustomerId
							select new { o.OrderId, o.OrderDate, c.Country, c.CompanyName }
						).Take(10).ToList();
					var list2 =
						(
							from o in db.Orders
							join c in db.Customers on o.CustomerId equals c.CustomerId
							select new { o.OrderId, o.OrderDate, c.Country, c.CompanyName }
						).Take(10).ToList();
					return list.Count + list2.Count;
				}
			};







			var testsToRun = new List<Tuple<Func<int>, string>>
			{
				//new Tuple<Func<int>, string>(simpleEf6CodeFirstTop10, "simple, Ef6CodeFirst, take 10"),
				//new Tuple<Func<int>, string>(simpleEf6CodeFirstTop500, "simple, Ef6CodeFirst, take 500"),
				//new Tuple<Func<int>, string>(simpleEf6CodeFirstRawTop10, "simple, Ef6CodeFirst raw, take 10"),
				//new Tuple<Func<int>, string>(simpleEf6CodeFirstRawTop500, "simple, Ef6CodeFirst raw, take 500"),
				//new Tuple<Func<int>, string>(simpleAdoNetTop10, "simple, ADO.NET, take 10"),
				//new Tuple<Func<int>, string>(simpleAdoNetTop500, "simple, ADO.NET, take 500"),
				//new Tuple<Func<int>, string>(simpleLinq2DbTop10, "simple, linq2db, take 10"),
				//new Tuple<Func<int>, string>(simpleLinq2DbTop500, "simple, linq2db, take 500"),
				//new Tuple<Func<int>, string>(simpleLinq2DbRawTop10, "simple, linq2db raw, take 10"),
				//new Tuple<Func<int>, string>(simpleLinq2DbRawTop500, "simple, linq2db raw, take 500"),
				new Tuple<Func<int>, string>(simpleEfCoreTop10, "simple, EfCore, take 10"),
				new Tuple<Func<int>, string>(simpleEfCoreTop500, "simple, EfCore, take 500"),
				new Tuple<Func<int>, string>(simpleEfCoreRawTop10, "simple, EfCore raw, take 10"),
				new Tuple<Func<int>, string>(simpleEfCoreRawTop500, "simple, EfCore raw, take 500"),
				new Tuple<Func<int>, string>(simpleEfCoreCompiledTop10, "simple, EfCore compiled, take 10"),
				new Tuple<Func<int>, string>(simpleEfCoreCompiledTop500, "simple, EfCore compiled, take 500"),

				new Tuple<Func<int>, string>(complexEf6CodeFirstTop10, "complex, Ef6CodeFirst, take 10"),
				new Tuple<Func<int>, string>(complexEf6CodeFirstTop500, "complex, Ef6CodeFirst, take 500"),
				//new Tuple<Func<int>, string>(complexEf6CodeFirstRawTop10, "complex, Ef6CodeFirst raw, take 10"),
				//new Tuple<Func<int>, string>(complexEf6CodeFirstRawTop500, "complex, Ef6CodeFirst raw, take 500"),
				new Tuple<Func<int>, string>(complexAdoNetTop10, "complex, ADO.NET, take 10"),
				new Tuple<Func<int>, string>(complexAdoNetTop500, "complex, ADO.NET, take 500"),
				new Tuple<Func<int>, string>(complexLinq2DbTop10, "complex, linq2db, take 10"),
				new Tuple<Func<int>, string>(complexLinq2DbTop500, "complex, linq2db, take 500"),
				new Tuple<Func<int>, string>(complexLinq2DbRawTop10, "complex, linq2db raw, take 10"),
				new Tuple<Func<int>, string>(complexLinq2DbRawTop500, "complex, linq2db raw, take 500"),
				new Tuple<Func<int>, string>(complexEfCoreTop10, "complex, EfCore, take 10"),
				new Tuple<Func<int>, string>(complexEfCoreTop500, "complex, EfCore, take 500"),

				new Tuple<Func<int>, string>(simpleEf6CodeFirstTop10And10, "simple, Ef6CodeFirst, take 10+10"),
				//new Tuple<Func<int>, string>(simpleEf6CodeFirstRawTop10And10, "simple, Ef6CodeFirst raw, take 10+10"),
				new Tuple<Func<int>, string>(simpleAdoNetTop10And10, "simple, ADO.NET, take 10+10"),
				new Tuple<Func<int>, string>(simpleLinq2DbTop10And10, "simple, linq2db, take 10+10"),
				new Tuple<Func<int>, string>(simpleLinq2DbRawTop10And10, "simple, linq2db raw, take 10+10"),
				new Tuple<Func<int>, string>(simpleEfCoreTop10And10, "simple, EfCore, take 10+10"),
			};







			Console.WriteLine("Tests configuration:");
			Console.WriteLine("cold = " + cold);
			Console.WriteLine("hot = " + hot);
			Console.WriteLine("---");
			

			foreach (var testItem in testsToRun)
			{
				var result = Test(testItem.Item1, cold, hot);
				Console.WriteLine("{0,-50} = {1,8} ms", testItem.Item2, result);
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

		class SimpleQueryRow
		{
			public int OrderId;
			public DateTime? OrderDate;
			public string Country;
			public string CompanyName;
		}

		class ComplexQueryRow
		{
			public short Quantity;
			public decimal UnitPrice;
			public float Discount;
			public string ShipCountry;
			public string Country;
		}
	}
}
