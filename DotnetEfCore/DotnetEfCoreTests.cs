using DotnetEfCore.CustomModels;
using DotnetEfCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DotnetEfCore
{
	public class DotnetEfCoreTests
	{
		public int SimpleTop10()
		{
			using (var db = new NorthwindContext())
			{
				var list =
					(
						from o in db.Orders
						join c in db.Customers on o.CustomerId equals c.CustomerId
						select new { o.OrderId, o.OrderDate, c.Country, c.CompanyName }
					).Take(10).ToList();
				return list.Count;
			}
		}

		public int SimpleTop500()
		{
			using (var db = new NorthwindContext())
			{
				var list =
					(
						from o in db.Orders
						join c in db.Customers on o.CustomerId equals c.CustomerId
						select new { o.OrderId, o.OrderDate, c.Country, c.CompanyName }
					).Take(500).ToList();
				return list.Count;
			}
		}

		public int SimpleRawTop10()
		{
			using (var db = new NorthwindContext())
			{
				var sql = @"
SELECT TOP 10 O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
				var list = db.SimpleQueryRows.FromSqlRaw(sql).ToList();
				return list.Count;
			}
		}

		public int SimpleRawTop500()
		{
			using (var db = new NorthwindContext())
			{
				var sql = @"
SELECT TOP 500 O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
				var list = db.SimpleQueryRows.FromSqlRaw(sql).ToList();
				return list.Count;
			}
		}

		public int ComplexTop10(int[] categoryIds, int[] supplierIds)
		{
			using (var db = new NorthwindContext())
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
		}

		public int ComplexTop500(int[] categoryIds, int[] supplierIds)
		{
			using (var db = new NorthwindContext())
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
		}

		public int ComplexRawTop10(int[] categoryIds, int[] supplierIds)
		{
			using (var db = new NorthwindContext())
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
				var list = db.ComplexQueryRows.FromSqlRaw(sql).ToList();
				return list.Count;
			}
		}

		public int ComplexRawTop500(int[] categoryIds, int[] supplierIds)
		{
			using (var db = new NorthwindContext())
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
				var list = db.ComplexQueryRows.FromSqlRaw(sql).ToList();
				return list.Count;
			}
		}

		public int SimpleTop10And10()
		{
			using (var db = new NorthwindContext())
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
		}

		public int SimpleRawTop10And10()
		{
			using (var db = new NorthwindContext())
			{
				var sql = @"
SELECT TOP 10 O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
				var list = db.SimpleQueryRows.FromSqlRaw(sql).ToList();

				sql = @"
SELECT TOP 10 O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
				var list2 = db.SimpleQueryRows.FromSqlRaw(sql).ToList();

				return list.Count + list2.Count;
			}
		}
	}
}
