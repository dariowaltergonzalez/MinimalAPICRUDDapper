using Dapper;
using Microsoft.Data.SqlClient;
using MinimalAPICRUDDapper.Entidades;
using System.Data;

namespace MinimalAPICRUDDapper.Repositorios
{
    public class RepositorioPersonas : IRepositorioPersonas
    {
        private readonly string connectionString;

        public RepositorioPersonas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Persona>> ObtenerTodos()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var personas = await connection.QueryAsync<Persona>("Personas_ObtenerTodos"
                    , commandType: CommandType.StoredProcedure);
                return personas;
            }
        }

        public async Task<Persona> ObtenerPorId(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var persona = await connection.QueryFirstOrDefaultAsync<Persona>("Personas_ObtenerPorId",
                new { id }
                , commandType: CommandType.StoredProcedure);
                return persona;
            }
        }

        public async Task<int> Crear(Persona persona)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var id = await connection.QuerySingleAsync<int>("Personas_Crear",
                new { persona.Nombre }
                , commandType: CommandType.StoredProcedure);
                return id;
            }
        }

        public async Task Actualizar(Persona persona)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync("Personas_Actualizar",
                    new { persona.Id, persona.Nombre }
                    , commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> ExistePorId(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var existe = await connection.QuerySingleAsync<bool>("Personas_ExistePorId",
                    new { id }
                    , commandType: CommandType.StoredProcedure);
                return existe;
            }
        }

        public async Task Borrar(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync("Personas_Borrar",
                    new { id }
                    , commandType: CommandType.StoredProcedure);
            }
        }
    }
}
