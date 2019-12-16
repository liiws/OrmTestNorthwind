using CoreLinq2db.CustomModels;
using LinqToDB.Data;
using System.Linq;

namespace CoreLinq2db
{
	public class CoreLinq2dbTests
	{
		public string Linq2dbConfiguration;

		public int SimpleTop10()
		{
			using (var db = new DataModels.NorthwindDB(Linq2dbConfiguration))
			{
				var list =
					(
						from o in db.Orders
						join c in db.Customers on o.CustomerID equals c.CustomerID
						select new { o.OrderID, o.OrderDate, c.Country, c.CompanyName }
					).Take(10).ToList();
				return list.Count;
			}
		}

		public int SimpleTop500()
		{
			using (var db = new DataModels.NorthwindDB(Linq2dbConfiguration))
			{
				var list =
					(
						from o in db.Orders
						join c in db.Customers on o.CustomerID equals c.CustomerID
						select new { o.OrderID, o.OrderDate, c.Country, c.CompanyName }
					).Take(500).ToList();
				return list.Count;
			}
		}

		public int SimpleRawTop10()
		{
			using (var db = new DataModels.NorthwindDB(Linq2dbConfiguration))
			{
				var sql = @"
SELECT TOP 10 O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
				var list = db.Query<SimpleQueryRow>(sql).ToList();
				return list.Count;
			}
		}

		public int SimpleRawTop500()
		{
			using (var db = new DataModels.NorthwindDB(Linq2dbConfiguration))
			{
				var sql = @"
SELECT TOP 500 O.OrderID, O.OrderDate, C.Country, C.CompanyName
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
							";
				var list = db.Query<SimpleQueryRow>(sql).ToList();
				return list.Count;
			}
		}

		public int ComplexTop10(int[] categoryIds, int[] supplierIds)
		{
			using (var db = new DataModels.NorthwindDB(Linq2dbConfiguration))
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
		}

		public int ComplexTop500(int[] categoryIds, int[] supplierIds)
		{
			using (var db = new DataModels.NorthwindDB(Linq2dbConfiguration))
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
		}

		public int ComplexRawTop10(int[] categoryIds, int[] supplierIds)
		{
			using (var db = new DataModels.NorthwindDB(Linq2dbConfiguration))
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
		}

		public int ComplexRawTop500(int[] categoryIds, int[] supplierIds)
		{
			using (var db = new DataModels.NorthwindDB(Linq2dbConfiguration))
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
		}

		public int SimpleTop10And10()
		{
			using (var db = new DataModels.NorthwindDB(Linq2dbConfiguration))
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
		}

		public int SimpleRawTop10And10()
		{
			using (var db = new DataModels.NorthwindDB(Linq2dbConfiguration))
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
		}
	}
}
