using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asp_servicios.Nucleo
{
    public class Pagos
    {
        [Key] public int Id { get; set; }
        public int IdPaga { get; set; }
        public int IdRecibe { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }

        [NotMapped] public Personas? _IdPaga { get; set; }
        [NotMapped] public Personas? _IdRecibe { get; set; }
    }
}
