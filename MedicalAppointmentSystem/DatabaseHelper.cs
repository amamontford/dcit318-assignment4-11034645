using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MedicalAppointmentSystem
{
    public static class DatabaseHelper
    {
        private static readonly string connectionString = InitializeConnectionString();

        private static string InitializeConnectionString()
        {
            try
            {
                var configured = ConfigurationManager.ConnectionStrings["MedicalDBConnection"]?.ConnectionString;
                if (!string.IsNullOrWhiteSpace(configured))
                {
                    return configured!;
                }

                // Fallback to LocalDB to avoid null reference and provide a sane default
                string fallback = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MedicalDB;Integrated Security=true;TrustServerCertificate=true;";
                MessageBox.Show(
                    "Connection string 'MedicalDBConnection' not found in App.config. Using LocalDB fallback.",
                    "Configuration Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return fallback;
            }
            catch (Exception ex)
            {
                // As a last resort, return a LocalDB fallback and surface the error
                MessageBox.Show($"Failed to read connection string: {ex.Message}. Falling back to LocalDB.", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MedicalDB;Integrated Security=true;TrustServerCertificate=true;";
            }
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static DataTable ExecuteQuery(string query, SqlParameter[]? parameters = null)
        {
            DataTable dataTable = new DataTable();
            
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dataTable;
        }

        public static int ExecuteNonQuery(string query, SqlParameter[]? parameters = null)
        {
            int rowsAffected = 0;
            
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return rowsAffected;
        }

        public static object ExecuteScalar(string query, SqlParameter[]? parameters = null)
        {
            object result = null;
            
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        result = command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }

        public static SqlDataReader ExecuteReader(string query, SqlParameter[]? parameters = null)
        {
            SqlConnection connection = GetConnection();
            SqlCommand command = new SqlCommand(query, connection);
            
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            try
            {
                connection.Open();
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show($"Database Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
