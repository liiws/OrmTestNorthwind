using DotnetAdo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DotnetAdo
{
	public class DotnetAdoTests
	{
		public string ConnectionStringNorthwind;

		public int SimpleTop10()
		{
			int count = 0;
			using (var con = new SqlConnection(ConnectionStringNorthwind))
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
		}

		public int SimpleTop500()
		{
			int count = 0;
			using (var con = new SqlConnection(ConnectionStringNorthwind))
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
		}

		public int ComplexTop10(int[] categoryIds, int[] supplierIds)
		{
			int count = 0;
			using (var con = new SqlConnection(ConnectionStringNorthwind))
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
		}

		public int ComplexTop500(int[] categoryIds, int[] supplierIds)
		{
			int count = 0;
			using (var con = new SqlConnection(ConnectionStringNorthwind))
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
		}

		public int SimpleTop10And10()
		{
			int count = 0;
			using (var con = new SqlConnection(ConnectionStringNorthwind))
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
		}
	}
}
