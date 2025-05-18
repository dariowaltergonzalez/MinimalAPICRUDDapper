using MinimalAPICRUDDapper.Entidades;

namespace MinimalAPICRUDDapper.Repositorios
{
    public interface IRepositorioPersonas
    {
        Task Actualizar(Persona persona);
        Task Borrar(int id);
        Task<int> Crear(Persona persona);
        Task<bool> ExistePorId(int id);
        Task<Persona> ObtenerPorId(int id);
        Task<IEnumerable<Persona>> ObtenerTodos();
    }
}