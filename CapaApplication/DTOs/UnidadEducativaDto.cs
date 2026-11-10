namespace CapaApplication.DTOs
{
    public class UnidadEducativaDto
    {
        public int IdUnidadEducativa { get; set; }
        public string? Nombre { get; set; }
        public string? NroContacto { get; set; }
        public string? Ubicacion { get; set; }
        public DateTime FechaCreado { get; set; }
        public string? Responsable { get; set; }
        public string FechaCreadoStr => FechaCreado.ToString("dd/MM/yyyy");
    }
}
