using CapaApplication.DTOs;
using CapaApplication.Responses;
using CapaApplication.Services;
using CapaWeb.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CapaWeb.Controllers
{
    public class DirectoresController : Controller
    {
        private readonly DirectorService _service;
        private readonly IPasswordHelper _passwordHelper;
        public DirectoresController(DirectorService service, IPasswordHelper passwordHelper)
        {
            _service = service;
            _passwordHelper = passwordHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListaDirectoresNormal()
        {
            var rpt = await _service.ListaNew(0);

            return StatusCode(StatusCodes.Status200OK, rpt);
        }

        [HttpGet]
        public async Task<IActionResult> ListaDirectoresConvert()
        {
            var rpt = await _service.Lista(0);

            return StatusCode(StatusCodes.Status200OK, rpt);
        }

        [HttpPut]
        public async Task<IActionResult> ObtenerDirector(int idDirector)
        {
            var rptbd = await _service.ObtenerDirector(idDirector);
            return StatusCode(StatusCodes.Status200OK, rptbd);
        }

        [HttpPut]
        public async Task<IActionResult> ObtenerDirectorConverter(int idDirector)
        {
            var rptbd = await _service.ObtenerDirectorConverter(idDirector);
            return StatusCode(StatusCodes.Status200OK, rptbd);
        }

        [HttpPost]
        public async Task<IActionResult> Guardar([FromForm] IFormFile? foto, [FromForm] string modelo)
        {
            try
            {
                // Deserializar DTO
                DirectorDto dto = JsonConvert.DeserializeObject<DirectorDto>(modelo)!;
                if (dto == null)
                {
                    Respuesta<int> rptEx = new() { Estado = false, Valor = "warning", Data = 0, Mensaje = "Debe proporcionar los datos del director" };
                    return StatusCode(StatusCodes.Status200OK, rptEx);
                }

                dto.ClaveHash = _passwordHelper.Hash(dto.NroCi!);

                // Si viene imagen → subirla
                if (foto != null)
                {
                    dto.Photo = "";
                    //dto.Photo = await _imageHelper.UploadImageAsync(foto, "images");
                }
                else
                {
                    dto.Photo = ""; // o null si lo prefieres
                }

                var rptbd = await _service.GuardarOrEditDirector(dto);
                return StatusCode(StatusCodes.Status200OK, rptbd);

            }
            catch (Exception ex)
            {
                Respuesta<int> rptEx = new() { Estado = false, Valor = "error", Data = 0, Mensaje = "Error en el servidor: " + ex.Message };
                return StatusCode(StatusCodes.Status200OK, rptEx);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromForm] IFormFile? foto, [FromForm] string modelo)
        {
            try
            {
                // Deserializar DTO
                DirectorDto dto = JsonConvert.DeserializeObject<DirectorDto>(modelo)!;

                // Si viene imagen → subirla
                if (foto != null)
                {
                    dto.Photo = "";
                    //dto.Photo = await _imageHelper.UploadImageAsync(foto, "images");
                }
                else
                {
                    dto.Photo = ""; // o null si lo prefieres
                }

                var rptbd = await _service.GuardarOrEditDirector(dto);
                return StatusCode(StatusCodes.Status200OK, rptbd);
            }
            catch (Exception ex)
            {
                Respuesta<int> rptEx = new() { Estado = false, Valor = "error", Data = 0, Mensaje = "Error en el servidor: " + ex.Message };
                return StatusCode(StatusCodes.Status200OK, rptEx);
            }
        }

    }
}
