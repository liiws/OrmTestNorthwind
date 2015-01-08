using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using EfDbContextDesigner.EfDbContextDesigner;
using OrmTestNorthwind.EfDbContextCodeFirst;
using OrmTestNorthwind.EfObjectContextEdmgen;
using System.Data.SqlClient;
using BLToolkit.Data;

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
            DbManager.AddConnectionString(connectionStringNorthwind);

            var categoryIds = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var supplierIds = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };



            // SIMPLE QUERY


            var simpleEfDbContextCodeFirstTop10 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("simple, EfDbContextCodeFirst, take 10 (n = {0}) = {1} ms", hot, simpleEfDbContextCodeFirstTop10);

            var simpleEfDbContextCodeFirstTop500 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("simple, EfDbContextCodeFirst, take 500 (n = {0}) = {1} ms", hot, simpleEfDbContextCodeFirstTop500);


            
            var simpleEfDbContextDesignerTop10 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("simple, EfDbContextDesigner, take 10 (n = {0}) = {1} ms", hot, simpleEfDbContextDesignerTop10);

            var simpleEfDbContextDesignerTop500 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("simple, EfDbContextDesigner, take 500 (n = {0}) = {1} ms", hot, simpleEfDbContextDesignerTop500);



            var simpleEfObjectContextEdmgenTop10 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("simple, EfObjectContextEdmgen, take 10 (n = {0}) = {1} ms", hot, simpleEfObjectContextEdmgenTop10);
            
            var simpleEfObjectContextEdmgenTop500 = Test(
                () =>
                {
                    using (var ctx = new EfObjectContextEdmgenEntities(connectionStringObjectContext))
                    {
                        ctx.Orders.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                        ctx.Customers.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                        var list =
                            (
                                from o in ctx.Orders
                                join c in ctx.Customers on o.CustomerID equals c.CustomerID
                                select new { o.OrderID, o.OrderDate, c.Country, c.CompanyName }
                            ).Take(500).ToList();
                        return list.Count;
                    }
                },
                cold,
                hot);
            Console.WriteLine("simple, EfObjectContextEdmgen, take 500 (n = {0}) = {1} ms", hot, simpleEfObjectContextEdmgenTop500);

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
            var simpleEfObjectContextEdmgenTop10Compiled = Test(
                () =>
                {
                    using (var ctx = new EfObjectContextEdmgenEntities(connectionStringObjectContext))
                    {
                        return querySimpleEfObjectContextEdmgenTop10Compiled(ctx).Count();
                    }
                },
                cold,
                hot);
            Console.WriteLine("simple compiled, EfObjectContextEdmgen, take 10 (n = {0}) = {1} ms", hot, simpleEfObjectContextEdmgenTop10Compiled);

            Func<EfObjectContextEdmgenEntities, IEnumerable<SimpleQueryRow>> querySimpleEfObjectContextEdmgenTop500Compiled =
                System.Data.Objects.CompiledQuery.Compile<EfObjectContextEdmgenEntities, IEnumerable<SimpleQueryRow>>
                    ((EfObjectContextEdmgenEntities ctx) =>
                            (
                                from o in ctx.Orders
                                join c in ctx.Customers on o.CustomerID equals c.CustomerID
                                select new { o.OrderID, o.OrderDate, c.Country, c.CompanyName }
                            ).Take(500)
                            .Select(r => new SimpleQueryRow
                            {
                                OrderId = r.OrderID,
                                OrderDate = r.OrderDate,
                                Country = r.Country,
                                CompanyName = r.CompanyName,
                            })
                    );
            var simpleEfObjectContextEdmgenTop500Compiled = Test(
                () =>
                {
                    using (var ctx = new EfObjectContextEdmgenEntities(connectionStringObjectContext))
                    {
                        return querySimpleEfObjectContextEdmgenTop500Compiled(ctx).Count();
                    }
                },
                cold,
                hot);
            Console.WriteLine("simple compiled, EfObjectContextEdmgen, take 500 (n = {0}) = {1} ms", hot, simpleEfObjectContextEdmgenTop500Compiled);




            var simpleAdoNetTop10 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("simple, ADO.NET, take 10 (n = {0}) = {1} ms", hot, simpleAdoNetTop10);

            var simpleAdoNetTop500 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("simple, ADO.NET, take 500 (n = {0}) = {1} ms", hot, simpleAdoNetTop500);



            var simpleBltTop10 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("simple, BLToolkit, take 10 (n = {0}) = {1} ms", hot, simpleBltTop10);

            var simpleBltTop500 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("simple, BLToolkit, take 500 (n = {0}) = {1} ms", hot, simpleBltTop500);



            // COMPLEX QUERY


            var complexEfDbContextCodeFirstTop10 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("complex, EfDbContextCodeFirst, take 10 (n = {0}) = {1} ms", hot, complexEfDbContextCodeFirstTop10);

            var complexEfDbContextCodeFirstTop500 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("complex, EfDbContextCodeFirst, take 500 (n = {0}) = {1} ms", hot, complexEfDbContextCodeFirstTop500);



            var complexEfDbContextDesignerTop10 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("complex, EfDbContextDesigner, take 10 (n = {0}) = {1} ms", hot, complexEfDbContextDesignerTop10);

            var complexEfDbContextDesignerTop500 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("complex, EfDbContextDesigner, take 500 (n = {0}) = {1} ms", hot, complexEfDbContextDesignerTop500);



            var complexEfObjectContextEdmgenTop10 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("complex, EfObjectContextEdmgen, take 10 (n = {0}) = {1} ms", hot, complexEfObjectContextEdmgenTop10);
            
            var complexEfObjectContextEdmgenTop500 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("complex, EfObjectContextEdmgen, take 500 (n = {0}) = {1} ms", hot, complexEfObjectContextEdmgenTop500);



            var complexAdoNetTop10 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("complex, ADO.NET, take 10 (n = {0}) = {1} ms", hot, complexAdoNetTop10);

            var complexAdoNetTop500 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("complex, ADO.NET, take 500 (n = {0}) = {1} ms", hot, complexAdoNetTop500);



            var complexBltTop10 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("complex, BLToolkit, take 10 (n = {0}) = {1} ms", hot, complexBltTop10);

            var complexBltTop500 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("complex, BLToolkit, take 500 (n = {0}) = {1} ms", hot, complexBltTop500);



            // SIMPLE 10 + 10 QUERY (TO CALCULATE CONTEXT INITIALIZATION TIME)

            
            var simpleEfDbContextCodeFirstTop10And10 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("simple, EfDbContextCodeFirst, take 10+10 (n = {0}) = {1} ms", hot, simpleEfDbContextCodeFirstTop10And10);

            var simpleEfDbContextDesignerTop10And10 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("simple, EfDbContextDesigner, take 10+10 (n = {0}) = {1} ms", hot, simpleEfDbContextDesignerTop10And10);

            var simpleEfObjectContextEdmgenTop10And10 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("simple, EfObjectContextEdmgen, take 10+10 (n = {0}) = {1} ms", hot, simpleEfObjectContextEdmgenTop10And10);

            var simpleAdoNetTop10And10 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("simple, ADO.NET, take 10+10 (n = {0}) = {1} ms", hot, simpleAdoNetTop10And10);

            var simpleBltTop10And10 = Test(
                () =>
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
                },
                cold,
                hot);
            Console.WriteLine("simple, BLToolkit, take 10+10 (n = {0}) = {1} ms", hot, simpleBltTop10And10);






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
