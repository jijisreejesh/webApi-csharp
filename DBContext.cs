using Npgsql;

public static class DBContext
{
    public static NpgsqlConnection GetConnection()
    {
        var _connectionString = "Host=192.168.1.10;Port=5432;Database=test;User Id=postgres;Password=PostgresDB@dm1n;";
        var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                // Set default schema for the session
                using (var command = new NpgsqlCommand("SET search_path TO jiji, public;", connection))
                {
                    command.ExecuteNonQuery();
                }
        return connection;
    }
}