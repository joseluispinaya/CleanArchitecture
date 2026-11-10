using CapaApplication.DTOs;
using CapaApplication.Responses;
using CapaDomain;

namespace CapaApplication.Interfaces
{
    public interface IUnidadEdRepository
    {
        Task<Respuesta<List<UnidadEducativa>>> ComboList(int idUniEd);
        Task<Respuesta<List<UnidadEducativaDto>>> ListaFull();
        Task<Respuesta<int>> GuardarOrEditUnidadesEdu(UnidadEducativa objeto);
    }
}
