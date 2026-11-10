using CapaApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace CapaWeb.Controllers
{
    public class UnidadEdController : Controller
    {
        private readonly UnidadEdService _service;
        public UnidadEdController(UnidadEdService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListaFullUnidades()
        {
            var rpt = await _service.ListaFull();

            // Retornamos directamente el objeto rpt. .NET lo serializa a JSON automáticamente.
            return StatusCode(StatusCodes.Status200OK, rpt);
        }

        [HttpGet]
        public async Task<IActionResult> ComboLista(int idUniEd)
        {
            // Esto llama a tu repositorio
            var response = await _service.ComboList(idUniEd);

            // Retornamos directamente el objeto response. .NET lo serializa a JSON automáticamente.
            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
