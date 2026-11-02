using CapaApplication.DTOs;
using CapaApplication.Helpers;
using CapaApplication.Interfaces;
using CapaApplication.Responses;

namespace CapaApplication.Services
{
    public class DirectorService
    {
        private readonly IDirectorRepository _repository;
        public DirectorService(IDirectorRepository repository)
        {
            _repository = repository;
        }

        //public Task<Respuesta<List<Director>>> ListaSinAsync(int idDirector) => _repository.Lista(idDirector);

        //public async Task<Respuesta<List<Director>>> Lista(int idDirector) => await _repository.Lista(idDirector);

        //public async Task<Respuesta<List<DirectorDto>>> Lista()
        //{
        //    Respuesta<List<DirectorDto>> resp = new()
        //    {
        //        Estado = false,
        //        Mensaje = "Error desconocido",
        //        Data = []
        //    };

        //    var rpt = await _repository.Lista();
        //    var listaEntidad = rpt.Data;

        //    var listaDto = listaEntidad!.Select(d => new DirectorDto
        //    {
        //        IdDirector = d.IdDirector,
        //        IdUnidadEducativa = d.IdUnidadEducativa,
        //        Nombres = d.Nombres,
        //        Apellidos = d.Apellidos,
        //        NroCi = d.NroCi,
        //        Celular = d.Celular,
        //        Correo = d.Correo,
        //        Estado = d.Estado,
        //        Photo = d.Photo,
        //        NombreUnidadEducativa = d.NombreUnidadEducativa
        //    }).ToList();

        //    resp.Estado = true;
        //    resp.Data = listaDto;
        //    return resp;
        //}

        public async Task<Respuesta<List<DirectorDto>>> Lista(int idDirector)
        {
            Respuesta<List<DirectorDto>> resp = new()
            {
                Estado = false,
                Mensaje = "Error desconocido",
                Data = []
            };

            var rpt = await _repository.Lista(idDirector);

            if (!rpt.Estado || rpt.Data == null)
            {
                resp.Mensaje = rpt.Mensaje;
                return resp;
            }

            // var listaEntidad = rpt.Data;
            //var listaDto = listaEntidad.Select(ConverterHelper.MapearDto).ToList();
            // resp.Data = listaDto;

            resp.Data = rpt.Data
                .Select(ConverterHelper.MapearDto)
                .ToList();

            resp.Estado = true;
            resp.Mensaje = rpt.Mensaje;

            return resp;
        }

        public async Task<Respuesta<DirectorDto>> ObtenerDirectorConverter(int idDirector)
        {
            Respuesta<DirectorDto> response = new()
            {
                Estado = false,
                Mensaje = "Error desconocido"
            };

            var rpt = await _repository.ObtenerDirector(idDirector);

            // Verificar si el repositorio devolvió un resultado válido
            if (!rpt.Estado || rpt.Data == null)
            {
                response.Mensaje = rpt.Mensaje;
                return response;
            }

            //var entidad = rpt.Data;

            //response.Data = MapearDto(entidad);

            // Convertir la entidad a DTO
            response.Data = ConverterHelper.MapearDto(rpt.Data);

            response.Estado = true;
            response.Mensaje = "Director obtenido correctamente";

            return response;
        }

        public async Task<Respuesta<List<DirectorDto>>> ListaNew(int idDirector)
        {
            Respuesta<List<DirectorDto>> resp = new()
            {
                Estado = false,
                Mensaje = "Error desconocido",
                Data = []
            };

            var rpt = await _repository.ListaNew(idDirector);

            if (!rpt.Estado || rpt.Data == null)
            {
                resp.Mensaje = rpt.Mensaje;
                return resp;
            }

            var listaEntidad = rpt.Data;

            var listaDto = listaEntidad.Select(d => new DirectorDto
            {
                IdDirector = d.IdDirector,
                IdUnidadEducativa = d.IdUnidadEducativa,
                Nombres = d.Nombres,
                Apellidos = d.Apellidos,
                NroCi = d.NroCi,
                Celular = d.Celular,
                Correo = d.Correo,
                Estado = d.Estado,
                Photo = d.Photo,
                NombreUnidadEducativa = d.NombreUnidadEducativa
            }).ToList();

            resp.Estado = true;
            resp.Mensaje = rpt.Mensaje;
            resp.Data = listaDto;
            return resp;
        }

        public async Task<Respuesta<DirectorDto>> ObtenerDirector(int idDirector)
        {
            Respuesta<DirectorDto> response = new()
            {
                Estado = false,
                Mensaje = "Error desconocido"
            };

            var rpt = await _repository.ObtenerDirector(idDirector);

            // Verificar si el repositorio devolvió un resultado válido
            if (!rpt.Estado || rpt.Data == null)
            {
                response.Mensaje = rpt.Mensaje;
                return response;
            }

            var entidad = rpt.Data;

            // Convertir la entidad a DTO
            response.Data = new DirectorDto
            {
                IdDirector = entidad.IdDirector,
                IdUnidadEducativa = entidad.IdUnidadEducativa,
                NombreUnidadEducativa = entidad.NombreUnidadEducativa,
                Nombres = entidad.Nombres,
                Apellidos = entidad.Apellidos,
                NroCi = entidad.NroCi,
                Celular = entidad.Celular,
                Correo = entidad.Correo,
                Photo = entidad.Photo,
                Estado = entidad.Estado
            };

            response.Estado = true;
            response.Mensaje = "Director obtenido correctamente";

            return response;
        }

        public async Task<Respuesta<int>> GuardarOrEditDirector(DirectorDto objeto)
        {
            // Convertir DTO a entidad
            //Director entidad = ConverterHelper.MapearEntidad(objeto);

            var entidad = ConverterHelper.MapearEntidad(objeto);
            // Enviar entidad al repositorio
            var rpt = await _repository.GuardarOrEditDirector(entidad);

            return rpt;
        }
    }
}
