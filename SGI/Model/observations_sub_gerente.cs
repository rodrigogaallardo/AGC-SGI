using System;
using System.Data;
using System.Data.SqlClient;

namespace SGI.GestionTramite.Controls
{
    public class Observaciones_sub_Gerente1
    {
        private readonly string _connectionString;

        // Constructor to initialize the connection string
        public Observaciones_sub_Gerente1(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Method to get observations based on id_tramitetarea and createUser
        public DataTable GetObservations_sub_gerente1(int? idTramitetarea, string createUser)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.GetObservations_sub_gerente1", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    if (idTramitetarea.HasValue)
                    {
                        command.Parameters.Add(new SqlParameter("@id_tramitetarea", idTramitetarea.Value));
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@id_tramitetarea", DBNull.Value));
                    }

                    if (!string.IsNullOrEmpty(createUser))
                    {
                        command.Parameters.Add(new SqlParameter("@CreateUser", createUser));
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@CreateUser", DBNull.Value));
                    }

                    // Open the connection and execute the command
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable observationsTable = new DataTable();
                        adapter.Fill(observationsTable);
                        return observationsTable;
                    }
                }
            }
        }
    }
}
