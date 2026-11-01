using CapaApplication.DTOs;
using CapaApplication.Interfaces;
using CapaApplication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaApplication.Services
{
    public class DirectorService
    {
        private readonly IDirectorRepository _repository;
        public DirectorService(IDirectorRepository repository)
        {
            _repository = repository;
        }

        public async Task<Respuesta<List<DirectorDto>>> Lista()
        {
            Respuesta<List<DirectorDto>> resp = new()
            {
                Estado = false,
                Mensaje = "Error desconocido",
                Data = []
            };

            var rpt = await _repository.Lista();
            var listaEntidad = rpt.Data;

            var listaDto = listaEntidad!.Select(d => new DirectorDto
            {
                IdDirector = d.IdDirector,
                IdUnidadEducativa = d.IdUnidadEducativa,
                Nombres = d.Nombres,
                Apellidos = d.Apellidos,
                NroCi = d.NroCi,
                Celular = d.Celular,
                Correo = d.Correo,
                NombreUnidadEducativa = d.NombreUnidadEducativa
            }).ToList();

            resp.Estado = true;
            resp.Data = listaDto;
            return resp;
        }
    }
}
