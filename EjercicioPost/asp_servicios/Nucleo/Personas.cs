using System.ComponentModel.DataAnnotations;

namespace asp_servicios.Nucleo
{
    public class Personas
    {
        [Key] public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Cedula { get; set; }
        public bool Activo { get; set; }
        public decimal Saldo { get; set; }
    }
}
