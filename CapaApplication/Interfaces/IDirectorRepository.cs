using CapaApplication.Responses;
using CapaDomain;

namespace CapaApplication.Interfaces
{
    public interface IDirectorRepository
    {
        Task<Respuesta<List<Director>>> Lista();
    }
}
