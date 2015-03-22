using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using EfDbContextDesigner.EfDbContextDesigner;
using LinqToDB.Data;
using OrmTestNorthwind.EfDbContextCodeFirst;
using OrmTestNorthwind.EfObjectContextEdmgen;
using System.Data.SqlClient;
using BLToolkit.Data;
using OrmTestNorthwind.Linq2Sql;

namespace OrmTestNorthwind
{
	class Program
	{
		static void Main(string[] args)
		{
			var cold = 100;
			var hot = 1000;

			var connectionStringObjectContext = ConfigurationManager.ConnectionStrings["EfObjectContextEdmgen"].ConnectionString;
			var connectionStringNorthwind = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;
			var linq2dbConfiguration = "NorthWindLinq2db";
			DbManager.AddConnectionString(connectionStringNorthwind);

			var categoryIds = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
			var supplierIds = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };



			// SIMPLE QUERY


			Func<int> simpleEfDbContextCodeFirstTop10 = () =>
			{
				using (var ctx = new NorthwindEfDbContextCodeFirst())
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

			Func<int> simpleEfDbContextCodeFirstTop500 = () =>
			{
				using (var ctx = new NorthwindEfDbContextCodeFirst())
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

			Func<int> simpleEfDbContextCodeFirstRawTop10 = () =>
			{
				using (var ctx = new NorthwindEfDbContextCodeFirst())
				{
					var sql = @"
SELECT TOP 10 O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
					var list = ctx.Database.SqlQuery<SimpleQueryRow>(sql).ToList();
					return list.Count;
				}
			};

			Func<int> simpleEfDbContextCodeFirstRawTop500 = () =>
			{
				using (var ctx = new NorthwindEfDbContextCodeFirst())
				{
					var sql = @"
SELECT TOP 500 O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
					var list = ctx.Database.SqlQuery<SimpleQueryRow>(sql).ToList();
					return list.Count;
				}
			};

			Func<int> simpleEfDbContextDesignerTop10 = () =>
			{
				using (var ctx = new NorthwindEfDbContextDesignerEntities())
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

			Func<int> simpleEfDbContextDesignerTop500 = () =>
			{
				using (var ctx = new NorthwindEfDbContextDesignerEntities())
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

			Func<int> simpleEfObjectContextEdmgenTop10 = () =>
			{
				using (var ctx = new EfObjectContextEdmgenEntities(connectionStringObjectContext))
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

			Func<int> simpleEfObjectContextEdmgenTop500 = () =>
			{
				using (var ctx = new EfObjectContextEdmgenEntities(connectionStringObjectContext))
				{
					//ctx.Orders.MergeOption = System.Data.Objects.MergeOption.NoTracking;
					//ctx.Customers.MergeOption = System.Data.Objects.MergeOption.NoTracking;
					var list =
						(
							from o in ctx.Orders
							join c in ctx.Customers on o.CustomerID equals c.CustomerID
							select new { o.OrderID, o.OrderDate, c.Country, c.CompanyName }
						).Take(500).ToList();
					return list.Count;
				}
			};

			Func<EfObjectContextEdmgenEntities, IEnumerable<SimpleQueryRow>> querySimpleEfObjectContextEdmgenTop10Compiled =
				System.Data.Objects.CompiledQuery.Compile<EfObjectContextEdmgenEntities, IEnumerable<SimpleQueryRow>>
					((EfObjectContextEdmgenEntities ctx) =>
							(
								from o in ctx.Orders
								join c in ctx.Customers on o.CustomerID equals c.CustomerID
								select new { o.OrderID, o.OrderDate, c.Country, c.CompanyName }
							).Take(10)
							.Select(r => new SimpleQueryRow
							{
								OrderId = r.OrderID,
								OrderDate = r.OrderDate,
								Country = r.Country,
								CompanyName = r.CompanyName,
							})
					);
			Func<int> simpleEfObjectContextEdmgenTop10Compiled = () =>
			{
				using (var ctx = new EfObjectContextEdmgenEntities(connectionStringObjectContext))
				{
					return querySimpleEfObjectContextEdmgenTop10Compiled(ctx).Count();
				}
			};

			Func<EfObjectContextEdmgenEntities, IEnumerable<SimpleQueryRow>> querySimpleEfObjectContextEdmgenTop500Compiled =
				System.Data.Objects.CompiledQuery.Compile<EfObjectContextEdmgenEntities, IEnumerable<SimpleQueryRow>>
					((EfObjectContextEdmgenEntities ctx) =>
							(
								from o in ctx.Orders
								join c in ctx.Customers on o.CustomerID equals c.CustomerID
								select new { o.OrderID, o.OrderDate, c.Country, c.CompanyName }
							).Take(10)
							.Select(r => new SimpleQueryRow
							{
								OrderId = r.OrderID,
								OrderDate = r.OrderDate,
								Country = r.Country,
								CompanyName = r.CompanyName,
							})
					);
			Func<int> simpleEfObjectContextEdmgenTop500Compiled = () =>
			{
				using (var ctx = new EfObjectContextEdmgenEntities(connectionStringObjectContext))
				{
					return querySimpleEfObjectContextEdmgenTop500Compiled(ctx).Count();
				}
			};

			Func<int> simpleLinq2SqlTop10 = () =>
			{
				using (var ctx = new Linq2SqlDataContext(connectionStringNorthwind))
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

			Func<int> simpleLinq2SqlTop500 = () =>
			{
				using (var ctx = new Linq2SqlDataContext(connectionStringNorthwind))
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

			Func<int> simpleBltTop10 = () =>
			{
				using (var db = new DbManager())
				{
					var sql = @"
SELECT TOP 10 O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
					var list = db.SetCommand(sql).ExecuteList<SimpleQueryRow>();
					return list.Count;
				}
			};

			Func<int> simpleBltTop500 = () =>
			{
				using (var db = new DbManager())
				{
					var sql = @"
SELECT TOP 500 O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
					var list = db.SetCommand(sql).ExecuteList<SimpleQueryRow>();
					return list.Count;
				}
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





			// COMPLEX QUERY



			Func<int> complexEfDbContextCodeFirstTop10 = () =>
			{
				using (var ctx = new NorthwindEfDbContextCodeFirst())
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

			Func<int> complexEfDbContextCodeFirstTop500 = () =>
			{
				using (var ctx = new NorthwindEfDbContextCodeFirst())
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

			Func<int> complexEfDbContextCodeFirstRawTop10 = () =>
			{
				using (var ctx = new NorthwindEfDbContextCodeFirst())
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
					var list = ctx.Database.SqlQuery<ComplexQueryRow>(sql).ToList();
					return list.Count;
				}
			};

			Func<int> complexEfDbContextCodeFirstRawTop500 = () =>
			{
				using (var ctx = new NorthwindEfDbContextCodeFirst())
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
					var list = ctx.Database.SqlQuery<ComplexQueryRow>(sql).ToList();
					return list.Count;
				}
			};


			Func<int> complexEfDbContextDesignerTop10 = () =>
			{
				using (var ctx = new NorthwindEfDbContextDesignerEntities())
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

			Func<int> complexEfDbContextDesignerTop500 = () =>
			{
				using (var ctx = new NorthwindEfDbContextDesignerEntities())
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

			Func<int> complexEfObjectContextEdmgenTop10 = () =>
			{
				using (var ctx = new EfObjectContextEdmgenEntities(connectionStringObjectContext))
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

			Func<int> complexEfObjectContextEdmgenTop500 = () =>
			{
				using (var ctx = new EfObjectContextEdmgenEntities(connectionStringObjectContext))
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

			Func<int> complexLinq2SqlTop10 = () =>
			{
				using (var ctx = new Linq2SqlDataContext(connectionStringNorthwind))
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

			Func<int> complexLinq2SqlTop500 = () =>
			{
				using (var ctx = new Linq2SqlDataContext(connectionStringNorthwind))
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

			Func<int> complexBltTop10 = () =>
			{
				using (var db = new DbManager())
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
					var list = db.SetCommand(sql).ExecuteList<ComplexQueryRow>();
					return list.Count;
				}
			};

			Func<int> complexBltTop500 = () =>
			{
				using (var db = new DbManager())
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
					var list = db.SetCommand(sql).ExecuteList<ComplexQueryRow>();
					return list.Count;
				}
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






			// SIMPLE 10 + 10 QUERY (TO CALCULATE CONTEXT INITIALIZATION TIME)



			Func<int> simpleEfDbContextCodeFirstTop10And10 = () =>
			{
				using (var ctx = new NorthwindEfDbContextCodeFirst())
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

			Func<int> simpleEfDbContextCodeFirstRawTop10And10 = () =>
			{
				using (var ctx = new NorthwindEfDbContextCodeFirst())
				{
					var sql = @"
SELECT TOP 10 O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
					var list = ctx.Database.SqlQuery<SimpleQueryRow>(sql).ToList();

					sql = @"
SELECT TOP 10 O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
					var list2 = ctx.Database.SqlQuery<SimpleQueryRow>(sql).ToList();

					return list.Count + list2.Count;
				}
			};

			Func<int> simpleEfDbContextDesignerTop10And10 = () =>
			{
				using (var ctx = new NorthwindEfDbContextDesignerEntities())
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

			Func<int> simpleEfObjectContextEdmgenTop10And10 = () =>
			{
				using (var ctx = new EfObjectContextEdmgenEntities(connectionStringObjectContext))
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

			Func<int> simpleLinq2SqlTop10And10 = () =>
			{
				using (var ctx = new Linq2SqlDataContext(connectionStringNorthwind))
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

			Func<int> simpleBltTop10And10 = () =>
			{
				using (var db = new DbManager())
				{
					var sql = @"
SELECT TOP 10 O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
					var list = db.SetCommand(sql).ExecuteList<SimpleQueryRow>();

					sql = @"
SELECT TOP 10 O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
					var list2 = db.SetCommand(sql).ExecuteList<SimpleQueryRow>();

					return list.Count + list2.Count;
				}
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







			var testsToRun = new List<Tuple<Func<int>, string>>
			{
				new Tuple<Func<int>, string>(simpleEfDbContextCodeFirstTop10, "simple, EfDbContextCodeFirst, take 10"),
				new Tuple<Func<int>, string>(simpleEfDbContextCodeFirstTop500, "simple, EfDbContextCodeFirst, take 500"),
				new Tuple<Func<int>, string>(simpleEfDbContextCodeFirstRawTop10, "simple, EfDbContextCodeFirst raw, take 10"),
				new Tuple<Func<int>, string>(simpleEfDbContextCodeFirstRawTop500, "simple, EfDbContextCodeFirst raw, take 500"),
				//new Tuple<Func<int>, string>(simpleEfDbContextDesignerTop10, "simple, EfDbContextDesigner, take 10"),
				//new Tuple<Func<int>, string>(simpleEfDbContextDesignerTop500, "simple, EfDbContextDesigner, take 500"),
				//new Tuple<Func<int>, string>(simpleEfObjectContextEdmgenTop10, "simple, EfObjectContextEdmgen, take 10"),
				//new Tuple<Func<int>, string>(simpleEfObjectContextEdmgenTop500, "simple, EfObjectContextEdmgen, take 500"),
				//new Tuple<Func<int>, string>(simpleEfObjectContextEdmgenTop10Compiled, "simple compiled, EfObjectContextEdmgen, take 10"),
				//new Tuple<Func<int>, string>(simpleEfObjectContextEdmgenTop500Compiled, "simple compiled, EfObjectContextEdmgen, take 500"),
				//new Tuple<Func<int>, string>(simpleLinq2SqlTop10, "simple, Linq2Sql, take 10"),
				//new Tuple<Func<int>, string>(simpleLinq2SqlTop500, "simple, Linq2Sql, take 500"),
				new Tuple<Func<int>, string>(simpleAdoNetTop10, "simple, ADO.NET, take 10"),
				new Tuple<Func<int>, string>(simpleAdoNetTop500, "simple, ADO.NET, take 500"),
				new Tuple<Func<int>, string>(simpleBltTop10, "simple, BLToolkit, take 10"),
				new Tuple<Func<int>, string>(simpleBltTop500, "simple, BLToolkit, take 500"),
				new Tuple<Func<int>, string>(simpleLinq2DbTop10, "simple, linq2db, take 10"),
				new Tuple<Func<int>, string>(simpleLinq2DbTop500, "simple, linq2db, take 500"),
				new Tuple<Func<int>, string>(simpleLinq2DbRawTop10, "simple, linq2db raw, take 10"),
				new Tuple<Func<int>, string>(simpleLinq2DbRawTop500, "simple, linq2db raw, take 500"),
				new Tuple<Func<int>, string>(complexEfDbContextCodeFirstTop10, "complex, EfDbContextCodeFirst, take 10"),
				new Tuple<Func<int>, string>(complexEfDbContextCodeFirstTop500, "complex, EfDbContextCodeFirst, take 500"),
				new Tuple<Func<int>, string>(complexEfDbContextCodeFirstRawTop10, "complex, EfDbContextCodeFirst raw, take 10"),
				new Tuple<Func<int>, string>(complexEfDbContextCodeFirstRawTop500, "complex, EfDbContextCodeFirst raw, take 500"),
				//new Tuple<Func<int>, string>(complexEfDbContextDesignerTop10, "complex, EfDbContextDesigner, take 10"),
				//new Tuple<Func<int>, string>(complexEfDbContextDesignerTop500, "complex, EfDbContextDesigner, take 500"),
				//new Tuple<Func<int>, string>(complexEfObjectContextEdmgenTop10, "complex, EfObjectContextEdmgen, take 10"),
				//new Tuple<Func<int>, string>(complexEfObjectContextEdmgenTop500, "complex, EfObjectContextEdmgen, take 500"),
				//new Tuple<Func<int>, string>(complexLinq2SqlTop10, "complex, Linq2Sql, take 10"),
				//new Tuple<Func<int>, string>(complexLinq2SqlTop500, "complex, Linq2Sql, take 500"),
				new Tuple<Func<int>, string>(complexAdoNetTop10, "complex, ADO.NET, take 10"),
				new Tuple<Func<int>, string>(complexAdoNetTop500, "complex, ADO.NET, take 500"),
				new Tuple<Func<int>, string>(complexBltTop10, "complex, BLToolkit, take 10"),
				new Tuple<Func<int>, string>(complexBltTop500, "complex, BLToolkit, take 500"),
				new Tuple<Func<int>, string>(complexLinq2DbTop10, "complex, linq2db, take 10"),
				new Tuple<Func<int>, string>(complexLinq2DbTop500, "complex, linq2db, take 500"),
				new Tuple<Func<int>, string>(complexLinq2DbRawTop10, "complex, linq2db raw, take 10"),
				new Tuple<Func<int>, string>(complexLinq2DbRawTop500, "complex, linq2db raw, take 500"),
				new Tuple<Func<int>, string>(simpleEfDbContextCodeFirstTop10And10, "simple, EfDbContextCodeFirst, take 10+10"),
				new Tuple<Func<int>, string>(simpleEfDbContextCodeFirstRawTop10And10, "simple, EfDbContextCodeFirst raw, take 10+10"),
				//new Tuple<Func<int>, string>(simpleEfDbContextDesignerTop10And10, "simple, EfDbContextDesigner, take 10+10"),
				//new Tuple<Func<int>, string>(simpleEfObjectContextEdmgenTop10And10, "simple, EfObjectContextEdmgen, take 10+10"),
				//new Tuple<Func<int>, string>(simpleLinq2SqlTop10And10, "simple, Linq2Sql, take 10+10"),
				new Tuple<Func<int>, string>(simpleAdoNetTop10And10, "simple, ADO.NET, take 10+10"),
				new Tuple<Func<int>, string>(simpleBltTop10And10, "simple, BLToolkit, take 10+10"),
				new Tuple<Func<int>, string>(simpleLinq2DbTop10And10, "simple, linq2db, take 10+10"),
				new Tuple<Func<int>, string>(simpleLinq2DbRawTop10And10, "simple, linq2db raw, take 10+10"),
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
