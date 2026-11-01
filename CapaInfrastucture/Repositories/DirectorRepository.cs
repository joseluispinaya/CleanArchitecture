using CapaApplication.Interfaces;
using CapaApplication.Responses;
using CapaDomain;
using CapaInfrastucture.DataBase;
using System.Data;

namespace CapaInfrastucture.Repositories
{
    public class DirectorRepository(DbConnectionFactory _connectionFactory) : IDirectorRepository
    {
        public async Task<Respuesta<List<Director>>> Lista()
        {
            Respuesta<List<Director>> rpt = new()
            {
                Estado = false,
                Mensaje = "Error desconocido",
                Data = []
            };

            try
            {
                using var conexion = _connectionFactory.CreateConnection();
                using var cmd = conexion.CreateCommand();

                cmd.CommandText = "usp_ObtenerDirectores";
                cmd.CommandType = CommandType.StoredProcedure;

                await conexion.OpenAsync();

                using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    rpt.Data.Add(new Director
                    {
                        IdDirector = Convert.ToInt32(dr["IdDirector"]),
                        IdUnidadEducativa = Convert.ToInt32(dr["IdUnidadEducativa"]),
                        NombreUnidadEducativa = dr["NombreUnidadEducativa"].ToString(),
                        Nombres = dr["Nombres"].ToString(),
                        Apellidos = dr["Apellidos"].ToString(),
                        NroCi = dr["NroCi"].ToString(),
                        Celular = dr["Celular"].ToString(),
                        Correo = dr["Correo"].ToString(),
                        Photo = dr["Photo"].ToString(),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    });
                }

                rpt.Estado = true;
                rpt.Mensaje = "Lista obtenida correctamente";
            }
            catch (Exception ex)
            {
                rpt.Mensaje = $"Error en BD: {ex.Message}";
            }

            return rpt;
        }
    }
}
