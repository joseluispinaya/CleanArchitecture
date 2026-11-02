using CapaApplication.Responses;
using CapaDomain;

namespace CapaApplication.Interfaces
{
    public interface IDirectorRepository
    {
        Task<Respuesta<List<Director>>> Lista(int idDirector);
        Task<Respuesta<List<Director>>> ListaNew(int idDirector);
        Task<Respuesta<Director>> ObtenerDirector(int idDirector);
        Task<Respuesta<int>> GuardarOrEditDirector(Director objeto);
    }
}
