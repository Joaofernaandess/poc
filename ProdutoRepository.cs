using Npgsql;
using Teste;

namespace Teste
{
    public class ProdutoRepository
    {
        private readonly string _connectionString; // criando uma variavel privada para guardar a string de conexão


        public ProdutoRepository(IConfiguration configuration)
        {
            // o configuration faz a conexão com o appsettings.jason guardando na nossa variavel privada _connectionString
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // método para criar um novo produto no banco de dados (famoso POST)
        public async Task CreateAsync(Produto produto)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql= @"INSERT INTO Produto (Nome, Estoque, Preco) 
                            VALUES(@Nome, @Estoque, @Preco) 
                            RETURNING Id";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Nome", produto.Nome ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Estoque", produto.Estoque);
                    command.Parameters.AddWithValue("@Preco", produto.Preco);

                    produto.Id = (Guid)await command.ExecuteScalarAsync();
                }
            }
        }

        public async Task<List<Produto>> GetAllAsync()
        {
            var lista = new List<Produto>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = "SELECT Id, Nome, Estoque, Preco FROM Produto";

                using (var command = new NpgsqlCommand(sql, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Produto
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            Nome = reader.IsDBNull(reader.GetOrdinal("Nome")) ? null : reader.GetString(reader.GetOrdinal("Nome")),
                            Estoque = reader.GetDecimal(reader.GetOrdinal("Estoque")),
                            Preco = reader.GetDecimal(reader.GetOrdinal("Preco"))
                        });
                    }
                }
            }
            return lista;
        }
    }
}
