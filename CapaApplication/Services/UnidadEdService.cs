using CapaApplication.DTOs;
using CapaApplication.Helpers;
using CapaApplication.Interfaces;
using CapaApplication.Responses;

namespace CapaApplication.Services
{
    public class UnidadEdService
    {
        private readonly IUnidadEdRepository _repository;
        public UnidadEdService(IUnidadEdRepository repository)
        {
            _repository = repository;
        }

        public async Task<Respuesta<int>> GuardarOrEditUnidadesEdu(UnidadEducativaDto objeto)
        {
            // Convertir DTO a entidad
            var entidad = ConverterHelper.MapearEntidadUniEd(objeto);
            // Enviar entidad al repositorio
            var rpt = await _repository.GuardarOrEditUnidadesEdu(entidad);

            return rpt;
        }

        public async Task<Respuesta<List<UnidadEducativaDto>>> ComboList(int idUniEd)
        {
            Respuesta<List<UnidadEducativaDto>> resp = new()
            {
                Estado = false,
                Mensaje = "Error desconocido",
                Data = []
            };

            var rpt = await _repository.ComboList(idUniEd);

            if (!rpt.Estado || rpt.Data == null)
            {
                resp.Mensaje = rpt.Mensaje;
                return resp;
            }

            var listaDto = rpt.Data.Select(d => new UnidadEducativaDto
            {
                IdUnidadEducativa = d.IdUnidadEducativa,
                Nombre = d.Nombre
            }).ToList();

            resp.Estado = true;
            resp.Mensaje = rpt.Mensaje;
            resp.Data = listaDto;
            return resp;
        }

        public async Task<Respuesta<List<UnidadEducativaDto>>> ListaFull() => await _repository.ListaFull();
    }
}
