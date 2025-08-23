using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PharmacyInventorySystem.Data
{
	public class DatabaseHelper : IDisposable
	{
		private readonly SqlConnection _connection;

		public DatabaseHelper()
		{
			string? connectionString = ConfigurationManager.ConnectionStrings["PharmacyDB"].ConnectionString;
			_connection = new SqlConnection(connectionString);
		}

		public SqlConnection Connection => _connection;

		public void Open()
		{
			if (_connection.State != ConnectionState.Open)
			{
				_connection.Open();
			}
		}

		public void Dispose()
		{
			if (_connection.State != ConnectionState.Closed)
			{
				_connection.Close();
			}
			_connection.Dispose();
		}

		public int AddMedicine(string name, string category, decimal price, int quantity)
		{
			using SqlCommand command = new SqlCommand("AddMedicine", _connection)
			{
				CommandType = CommandType.StoredProcedure
			};
			command.Parameters.AddWithValue("@Name", name);
			command.Parameters.AddWithValue("@Category", category);
			command.Parameters.AddWithValue("@Price", price);
			command.Parameters.AddWithValue("@Quantity", quantity);
			return command.ExecuteNonQuery();
		}

		public DataTable SearchMedicine(string searchTerm)
		{
			using SqlCommand command = new SqlCommand("SearchMedicine", _connection)
			{
				CommandType = CommandType.StoredProcedure
			};
			command.Parameters.AddWithValue("@SearchTerm", searchTerm);
			using SqlDataReader reader = command.ExecuteReader();
			DataTable table = new DataTable();
			table.Load(reader);
			return table;
		}

		public int UpdateStock(int medicineId, int quantity)
		{
			using SqlCommand command = new SqlCommand("UpdateStock", _connection)
			{
				CommandType = CommandType.StoredProcedure
			};
			command.Parameters.AddWithValue("@MedicineID", medicineId);
			command.Parameters.AddWithValue("@Quantity", quantity);
			return command.ExecuteNonQuery();
		}

		public int RecordSale(int medicineId, int quantitySold)
		{
			using SqlCommand command = new SqlCommand("RecordSale", _connection)
			{
				CommandType = CommandType.StoredProcedure
			};
			command.Parameters.AddWithValue("@MedicineID", medicineId);
			command.Parameters.AddWithValue("@QuantitySold", quantitySold);
			return command.ExecuteNonQuery();
		}

		public DataTable GetAllMedicines()
		{
			using SqlCommand command = new SqlCommand("GetAllMedicines", _connection)
			{
				CommandType = CommandType.StoredProcedure
			};
			using SqlDataReader reader = command.ExecuteReader();
			DataTable table = new DataTable();
			table.Load(reader);
			return table;
		}

		public int UpdateMedicine(int medicineId, string name, string category, decimal price, int quantity)
		{
			using SqlCommand command = new SqlCommand("UpdateMedicine", _connection)
			{
				CommandType = CommandType.StoredProcedure
			};
			command.Parameters.AddWithValue("@MedicineID", medicineId);
			command.Parameters.AddWithValue("@Name", name);
			command.Parameters.AddWithValue("@Category", category);
			command.Parameters.AddWithValue("@Price", price);
			command.Parameters.AddWithValue("@Quantity", quantity);
			return command.ExecuteNonQuery();
		}

		public DataTable GetSales()
		{
			using SqlCommand command = new SqlCommand("GetSales", _connection)
			{
				CommandType = CommandType.StoredProcedure
			};
			using SqlDataReader reader = command.ExecuteReader();
			DataTable table = new DataTable();
			table.Load(reader);
			return table;
		}
	}
}


