using Npgsql;
using Teste;
namespace Teste
{
    public class ClienteRepository
    
    {
        private readonly string _connectionString;
        public ClienteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // post \/
        public async Task CreateAsync(Cliente cliente)
        {
            using (var connection  = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = @"INSERT INTO ""Cliente"" (""Nome"", ""Cpf"", ""Telefone"" , ""Email"") 
                VALUES (@Nome, CAST(@Cpf AS INTEGER) , CAST(@Tel AS INTEGER), @Email)
                RETURNING ""Id"" ";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Nome", cliente.Nome);
                    command.Parameters.AddWithValue("@Cpf", cliente.Cpf ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Tel", cliente.Telefone ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Email", cliente.Email ?? (object)DBNull.Value);
                    cliente.Id = (Guid)await command.ExecuteScalarAsync();
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
                var sql = @"Select * FROM ""Cliente"" ";
                using (var command = new NpgsqlCommand(sql, connection ))
                using ( var reader = await command.ExecuteReaderAsync())
                {
                    while(await reader.ReadAsync())
                    {
                        lista.Add(new Cliente
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            Nome = reader.GetString(reader.GetOrdinal("Nome")),
                            Cpf = reader.IsDBNull(reader.GetOrdinal("Cpf")) ? null: reader.GetString(reader.GetOrdinal("Cpf")),
                            Telefone = reader.IsDBNull(reader.GetOrdinal("Telefone")) ? null: reader.GetString(reader.GetOrdinal("Telefone")),
                            Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null: reader.GetString(reader.GetOrdinal("Email")),

                        });
                    }
                }              
            }
            return lista;           
        }
        public async Task<Cliente> GetByIdAsync(Guid id)
        {
            Cliente clienteEncontrado = null;

            using(var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"SELECT * FROM ""Cliente"" WHERE Id = @Id";
                using(var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if(await reader.ReadAsync())
                        {
                            clienteEncontrado = new Cliente
                            {
                                Id = reader.GetGuid (reader.GetOrdinal("Id")),
                                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                Cpf = reader.IsDBNull(reader.GetOrdinal("Cpf"))? null : reader.GetString(reader.GetOrdinal ("Cpf")),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email"))? null : reader.GetString(reader.GetOrdinal("Email")),
                                Telefone = reader.IsDBNull(reader.GetOrdinal("Telefone"))? null : reader.GetString(reader.GetOrdinal
                                ("Telefone"))
                            };
                        }
                    }

                }
            }
            return clienteEncontrado;
        }
        
        public async Task DeleteAsync(Guid id)
        {
                using(var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var sql = @"DELETE FROM ""Cliente"" WHERE Id = @Id";
                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        await command.ExecuteNonQueryAsync();
                    }
                }
        }
    }
}
