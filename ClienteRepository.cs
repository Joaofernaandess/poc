using Npgsql;
using GestãoApi;
using System.Runtime.CompilerServices;
using System.Data.SqlTypes;
using System.Security.Cryptography.X509Certificates;
using System.Dynamic;
namespace GestãoApi
{
    public class ClienteRepository
    {
        private readonly string _connectionString;
        public ClienteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DeafaultConnection");
        
        }
        // post \/
        public async Task CreatAsync(Cliente cliente)
        {
            using (var connection  = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = @"INSERT INTO Clientes (Nome, Cpf, Telefone, Email) 
                VALUES (@Nome, @Cpf, @Tel, @Email)
                RETURNING Id";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Nome", cliente.Nome);
                    command.Parameters.AddWithValue("@Cpf", cliente.Cpf ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@tel", cliente.Telefone ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Email", cliente.Email ?? (object)DBNull.Value);
                    cliente.Clienteid = (Guid)await command.ExecuteScalarAsync();
                }
            }
        }
        // (GET \/)
        public async Task<List<Cliente>> GetAllAsync()
        {
            var lista = new List<Cliente>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "Select *FROM Clientes";
                using (var command = new NpgsqlCommand(sql, connection ))
                using ( var reader = await command.ExecuteReaderAsync())
                {
                    while(await reader.ReadAsync())
                    {
                        lista.Add(new Cliente
                        {
                            Clienteid = reader.GetGuid(reader.GetOrdinal("Id")),
                            Nome = reader.GetString(reader.GetOrdinal("Nome")),
                            Cpf = reader.IsDBNull(reader.GetOrdinal("Cpf")) ? null: reader.GetString(reader.GetOrdinal("cpf")),
                            Telefone = reader.IsDBNull(reader.GetOrdinal("Telefone")) ? null: reader.GetString(reader.GetOrdinal("Telefone")),
                            Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null: reader.GetString(reader.GetOrdinal("Email")),

                        });
                    }
                }              
            }
            return lista;           
        }
        public async Task DeleteAsync(Guid id)
            {
                using(var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var sql = "DELETE FROM Cliente WHERE Id = @Id";
                using (var command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
    }
}
