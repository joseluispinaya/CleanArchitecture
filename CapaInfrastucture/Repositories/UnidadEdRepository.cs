using CapaApplication.DTOs;
using CapaApplication.Interfaces;
using CapaApplication.Responses;
using CapaDomain;
using CapaInfrastucture.DataBase;
using System.Data;

namespace CapaInfrastucture.Repositories
{
    public class UnidadEdRepository(DbConnectionFactory _connectionFactory) : IUnidadEdRepository
    {

        public async Task<Respuesta<int>> GuardarOrEditUnidadesEdu(UnidadEducativa objeto)
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

                cmd.CommandText = "usp_GuardarOrEditUnidadEducativa";
                cmd.CommandType = CommandType.StoredProcedure;

                // Parámetros de entrada
                cmd.Parameters.AddWithValue("@IdUnidadEducativa", objeto.IdUnidadEducativa);
                cmd.Parameters.AddWithValue("@Nombre", objeto.Nombre);
                cmd.Parameters.AddWithValue("@NroContacto", objeto.NroContacto);
                cmd.Parameters.AddWithValue("@Ubicacion", objeto.Ubicacion);

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
                    case 1: // Duplicado
                        response.Estado = false;
                        response.Valor = "warning"; // <--- Usamos Valor para el icono AMARILLO
                        response.Mensaje = "El nombre de la unidad educativa ya existe.";
                        break;

                    case 2: // Registro Nuevo
                        response.Estado = true;
                        response.Valor = "success"; // <--- Usamos Valor para el icono VERDE
                        response.Mensaje = "Unidad educativa registrada correctamente.";
                        break;

                    case 3: // Actualización
                        response.Estado = true;
                        response.Valor = "success"; // <--- Usamos Valor para el icono VERDE
                        response.Mensaje = "Unidad educativa actualizada correctamente.";
                        break;

                    case 0: // Error
                    default:
                        response.Estado = false;
                        response.Valor = "error"; // <--- Usamos Valor para el icono ROJO
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

        public async Task<Respuesta<List<UnidadEducativa>>> ComboList(int idUniEd)
        {
            Respuesta<List<UnidadEducativa>> rpt = new()
            {
                Estado = false,
                Mensaje = "Error desconocido",
                Data = []
            };

            try
            {
                using var conexion = _connectionFactory.CreateConnection();
                using var cmd = conexion.CreateCommand();

                cmd.CommandText = "sp_ListarUnidadesEducativasDisponibles";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdUnidadEducativaActual", idUniEd);

                await conexion.OpenAsync();

                using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    rpt.Data.Add(new UnidadEducativa
                    {
                        IdUnidadEducativa = Convert.ToInt32(dr["IdUnidadEducativa"]),
                        Nombre = dr["Nombre"].ToString()
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


        public async Task<Respuesta<List<UnidadEducativaDto>>> ListaFull()
        {
            Respuesta<List<UnidadEducativaDto>> rpt = new()
            {
                Estado = false,
                Mensaje = "Error desconocido",
                Data = []
            };

            try
            {
                using var conexion = _connectionFactory.CreateConnection();
                using var cmd = conexion.CreateCommand();

                cmd.CommandText = "usp_ListarUnidadEducativaFull";
                cmd.CommandType = CommandType.StoredProcedure;

                await conexion.OpenAsync();

                using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    rpt.Data.Add(new UnidadEducativaDto
                    {
                        IdUnidadEducativa = Convert.ToInt32(dr["IdUnidadEducativa"]),
                        Nombre = dr["Nombre"].ToString(),
                        NroContacto = dr["NroContacto"].ToString(),
                        Ubicacion = dr["Ubicacion"].ToString(),
                        FechaCreado = Convert.ToDateTime(dr["FechaCreado"]),
                        Responsable = dr["Responsable"].ToString()
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
