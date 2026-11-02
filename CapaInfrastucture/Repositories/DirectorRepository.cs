using CapaApplication.Interfaces;
using CapaApplication.Responses;
using CapaDomain;
using CapaInfrastucture.DataBase;
using System.Data;

namespace CapaInfrastucture.Repositories
{
    public class DirectorRepository(DbConnectionFactory _connectionFactory) : IDirectorRepository
    {
        public async Task<Respuesta<int>> GuardarOrEditDirector(Director objeto)
        {
            Respuesta<int> response = new()
            {
                Estado = false,
                Valor = "warning",
                Data = 0,
                Mensaje = "Error desconocido"
            };

            try
            {
                using var conexion = _connectionFactory.CreateConnection();
                using var cmd = conexion.CreateCommand();

                cmd.CommandText = "usp_GuardarOEditarDirector";
                cmd.CommandType = CommandType.StoredProcedure;

                // Parámetros de entrada
                cmd.Parameters.AddWithValue("@IdDirector", objeto.IdDirector);
                cmd.Parameters.AddWithValue("@IdUnidadEducativa", objeto.IdUnidadEducativa);
                cmd.Parameters.AddWithValue("@Nombres", objeto.Nombres);
                cmd.Parameters.AddWithValue("@Apellidos", objeto.Apellidos);
                cmd.Parameters.AddWithValue("@NroCi", objeto.NroCi);
                cmd.Parameters.AddWithValue("@Celular", objeto.Celular);
                cmd.Parameters.AddWithValue("@Correo", objeto.Correo);
                cmd.Parameters.AddWithValue("@ClaveHash", string.IsNullOrEmpty(objeto.ClaveHash) ? "" : objeto.ClaveHash);
                cmd.Parameters.AddWithValue("@Photo", string.IsNullOrEmpty(objeto.Photo) ? "" : objeto.Photo);

                // Parámetro de salida
                var outputParam = cmd.Parameters.Add("@Resultado", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;

                // Abrir conexión y ejecutar procedimiento
                await conexion.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                // Obtener resultado del procedimiento
                int resultadoCodigo = Convert.ToInt32(outputParam.Value);

                response.Data = resultadoCodigo;

                switch (resultadoCodigo)
                {
                    case 1:
                        response.Estado = false;
                        response.Valor = "warning";
                        response.Mensaje = "El nro de CI o el correo ya existe.";
                        break;

                    case 2:
                        response.Estado = true;
                        response.Valor = "success";
                        response.Mensaje = "Registrado correctamente.";
                        break;

                    case 3:
                        response.Estado = true;
                        response.Valor = "success";
                        response.Mensaje = "Actualizado correctamente.";
                        break;

                    case 4:
                        response.Estado = false;
                        response.Valor = "warning";
                        response.Mensaje = "La unidad educativa seleccionada ya está asignada a un director.";
                        break;

                    case 0:
                    default:
                        response.Estado = false;
                        response.Valor = "error";
                        response.Mensaje = "No se pudo completar la operación.";
                        break;
                }
            }
            catch (Exception ex)
            {
                response.Estado = false;
                response.Valor = "error";
                response.Mensaje = $"Error en BD: {ex.Message}";
            }

            return response;
        }

        public async Task<Respuesta<List<Director>>> Lista(int idDirector)
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

                cmd.Parameters.AddWithValue("@IdDirector", idDirector);

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

        public async Task<Respuesta<List<Director>>> ListaNew(int idDirector)
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

                cmd.Parameters.AddWithValue("@IdDirector", idDirector);

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

        public async Task<Respuesta<Director>> ObtenerDirector(int idDirector)
        {
            Respuesta<Director> response = new()
            {
                Estado = false,
                Mensaje = "Error desconocido"
            };

            try
            {
                using var conexion = _connectionFactory.CreateConnection();
                using var cmd = conexion.CreateCommand();

                cmd.CommandText = "usp_ObtenerDirectores";
                cmd.CommandType = CommandType.StoredProcedure;

                //cmd.Parameters.Add("@IdDirector", SqlDbType.Int).Value = idDirector;
                cmd.Parameters.AddWithValue("@IdDirector", idDirector);

                await conexion.OpenAsync();

                using var dr = await cmd.ExecuteReaderAsync();

                if (await dr.ReadAsync())
                {
                    response.Data = new Director
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
                    };

                    response.Estado = true;
                    response.Mensaje = "Director obtenido correctamente";
                }
                else
                {
                    response.Mensaje = "No se encontró el director";
                }
            }
            catch (Exception ex)
            {
                response.Mensaje = $"Error en BD: {ex.Message}";
            }

            return response;
        }
    }
}
