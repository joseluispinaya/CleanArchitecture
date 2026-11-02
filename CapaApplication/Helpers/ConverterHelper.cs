using CapaApplication.DTOs;
using CapaDomain;

namespace CapaApplication.Helpers
{
    public class ConverterHelper
    {
        public static DirectorDto MapearDto(Director entidad)
        {
            return new DirectorDto
            {
                IdDirector = entidad.IdDirector,
                IdUnidadEducativa = entidad.IdUnidadEducativa,
                NombreUnidadEducativa = entidad.NombreUnidadEducativa,
                Nombres = entidad.Nombres,
                Apellidos = entidad.Apellidos,
                NroCi = entidad.NroCi,
                Celular = entidad.Celular,
                Correo = entidad.Correo,
                ClaveHash = entidad.ClaveHash,
                Photo = entidad.Photo,
                Estado = entidad.Estado
            };
        }

        // public static Director MapearEntidad(DirectorDto modelo, bool isNew)
        public static Director MapearEntidad(DirectorDto modelo)
        {
            return new Director
            {
                IdDirector = modelo.IdDirector,
                //IdDirector = isNew ? 0 : model.IdDirector,
                IdUnidadEducativa = modelo.IdUnidadEducativa,
                NombreUnidadEducativa = modelo.NombreUnidadEducativa,
                Nombres = modelo.Nombres,
                Apellidos = modelo.Apellidos,
                NroCi = modelo.NroCi,
                Celular = modelo.Celular,
                Correo = modelo.Correo,
                ClaveHash = modelo.ClaveHash,
                Photo = modelo.Photo,
                Estado = modelo.Estado
            };
        }
    }
}
