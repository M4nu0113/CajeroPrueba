using asp_servicios.Nucleo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lib_utilidades;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace asp_servicios.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PersonasController : ControllerBase
    {
        private string cadena = "server=M4NU_HELPER\\DEV;database=db_ejercicio;uid=sa;pwd=STEMgirls>>>;TrustServerCertificate=true;";
        public PersonasController()
        {
            var conexion = new Conexion();
            conexion.StringConnection = cadena;
            conexion.Database.Migrate();
        }
        private Dictionary<string, object> ObtenerDatos()
        {
            var datos = new StreamReader(Request.Body).ReadToEnd().ToString();
            if (string.IsNullOrEmpty(datos))
                return new Dictionary<string, object>();
            return JsonHelper.ConvertirAObjeto(datos);
        }

        [HttpPost]
        public string Listar()
        {
            var conexion = new Conexion();
            conexion.StringConnection = cadena;
            var respuesta = new Dictionary<string, object>();
            try
            {
                var datos = ObtenerDatos();
                respuesta["Entidades"] = conexion.Listar<Personas>();
                respuesta["Respuesta"] = "OK";
                respuesta["Fecha"] = DateTime.Now.ToString();
                return JsonHelper.ConvertirAString(respuesta);
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.Message.ToString();
                return JsonHelper.ConvertirAString(respuesta);
            }
        }

        [HttpPost]
        public string Guardar()
        {
            var conexion = new Conexion();
            conexion.StringConnection = cadena;
            var respuesta = new Dictionary<string, object>();
            try
            {
                var datos = ObtenerDatos();

                var entidad = JsonHelper.ConvertirAObjeto<Personas>(
                JsonHelper.ConvertirAString(datos["Entidad"]));
                conexion.Guardar(entidad);
                conexion.GuardarCambios();

                respuesta["Entidad"] = entidad;
                respuesta["Respuesta"] = "OK";
                respuesta["Fecha"] = DateTime.Now.ToString();
                return JsonHelper.ConvertirAString(respuesta);
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.Message.ToString();
                return JsonHelper.ConvertirAString(respuesta);
            }
        }

        [HttpPost]
        public string CantPersonas()
        {
            var conexion = new Conexion();
            conexion.StringConnection = cadena;
            int atendidas = conexion.Listar<Personas>().Count;
            var respuesta = new Dictionary<string, object>();

            respuesta["Cantidad de personas atendidas"] = atendidas;
            return JsonHelper.ConvertirAString(respuesta);
        }

        [HttpPost]
        public string Borrar()
        {
            var conexion = new Conexion();
            conexion.StringConnection = cadena;

            var respuesta = new Dictionary<string, object>();
            try
            {
                var datos = ObtenerDatos();

                var entidad = JsonHelper.ConvertirAObjeto<Personas>(
                    JsonHelper.ConvertirAString(datos["Entidad"]));
                conexion.Borrar(entidad);
                conexion.GuardarCambios();

                respuesta["Entidad"] = entidad;
                respuesta["Respuesta"] = "OK";
                respuesta["Fecha"] = DateTime.Now.ToString();
                return JsonHelper.ConvertirAString(respuesta);
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.Message.ToString();
                return JsonHelper.ConvertirAString(respuesta);
            }
        }

        [HttpPost]
        public string Modificar()
        {
            var conexion = new Conexion();
            conexion.StringConnection = cadena;
            var respuesta = new Dictionary<string, object>();
            try
            {
                var datos = ObtenerDatos();

                var entidad = JsonHelper.ConvertirAObjeto<Personas>(
                    JsonHelper.ConvertirAString(datos["Entidad"]));
                conexion.Modificar(entidad);
                conexion.GuardarCambios();

                respuesta["Entidad"] = entidad;
                respuesta["Respuesta"] = "OK";
                respuesta["Fecha"] = DateTime.Now.ToString();
                return JsonHelper.ConvertirAString(respuesta);
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.Message.ToString();
                return JsonHelper.ConvertirAString(respuesta);
            }
        }

        [HttpPost]
        public string FiltrarSaldo()
        {
            var conexion = new Conexion();
            conexion.StringConnection = cadena;
            var respuesta = new Dictionary<string, object>();
            List<Personas> FiltroSalario = new List<Personas>();

            try
            {

                foreach (var persona in conexion.Listar<Personas>())
                {
                    if (persona.Saldo > 1300000)
                    {
                        FiltroSalario.Add(persona);
                    }
                }
                respuesta["Entidades"] = FiltroSalario;
                return JsonHelper.ConvertirAString(respuesta);
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.Message.ToString();
                return JsonHelper.ConvertirAString(respuesta);
            }
        }

        [HttpPost]
        public string ListarPagos()
        {
            var conexion = new Conexion();
            conexion.StringConnection = cadena;
            var respuesta = new Dictionary<string, object>();
            try
            {
                var datos = ObtenerDatos();
                respuesta["Entidades"] = conexion.Listar<Pagos>();
                respuesta["Respuesta"] = "OK";
                respuesta["Fecha"] = DateTime.Now.ToString();
                return JsonHelper.ConvertirAString(respuesta);
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.Message.ToString();
                return JsonHelper.ConvertirAString(respuesta);
            }
        }

        [HttpPost]
        public string GuardarPagos()
        {
            var conexion = new Conexion();
            conexion.StringConnection = cadena;
            var respuesta = new Dictionary<string, object>();
            try
            {
                var datos = ObtenerDatos();

                var entidad = JsonHelper.ConvertirAObjeto<Pagos>(
                JsonHelper.ConvertirAString(datos["Entidad"]));
                conexion.Guardar(entidad);
                conexion.GuardarCambios();

                respuesta["Entidad"] = entidad;
                respuesta["Respuesta"] = "OK";
                respuesta["Fecha"] = DateTime.Now.ToString();
                return JsonHelper.ConvertirAString(respuesta);
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.Message.ToString();
                return JsonHelper.ConvertirAString(respuesta);
            }
        }

        [HttpPost]
        public string PromedioPagan()
        {
            var conexion = new Conexion();
            conexion.StringConnection = cadena;
            // lista para agregar a las personas que pagan
            List<Pagos> pagan = new List<Pagos>();
            var IdPagaSet = new HashSet<int>();
            var respuesta = new Dictionary<string, object>();

            try
            {

                // recorrer la lista de pagos y verificar que no se repitan
                foreach (var pago in conexion.Listar<Pagos>())
                {
                    if (IdPagaSet.Add(pago.IdPaga))
                    {
                        pagan.Add(pago);
                    }
                }
                // obtener el promedio de las personas que pagan
                double cantpagan = pagan.Count();
                double personas = conexion.Listar<Personas>().Count();
                double promedio = cantpagan / personas;
                respuesta["CantPagan"] = pagan.Count();
                respuesta["CantPersonas"] = conexion.Listar<Personas>().Count();
                respuesta["Promedio de personas que pagan"] = promedio;
                return JsonHelper.ConvertirAString(respuesta);
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.Message.ToString();
                return JsonHelper.ConvertirAString(respuesta);

            }
        }

        [HttpPost]
        public string FiltrarPagosFecha()
        {
            var conexion = new Conexion();
            conexion.StringConnection = cadena;
            var respuesta = new Dictionary<string, object>();
            List<Pagos> FiltroPagos = new List<Pagos>();

            try
            {
                var datos = ObtenerDatos();

                DateTime fechaInicio = new DateTime(2011, 01, 01);
                DateTime fechaFin = new DateTime(2018, 12, 31);

                foreach (var pago in conexion.Listar<Pagos>())
                {
                    if (pago.Fecha >= fechaInicio && pago.Fecha <= fechaFin)
                    {
                        FiltroPagos.Add(pago);
                    }
                }
                respuesta["FechaInicio"] = fechaInicio;
                respuesta["FechaFin"] = fechaFin;
                respuesta["Entidades"] = FiltroPagos;
                return JsonHelper.ConvertirAString(respuesta);
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.Message.ToString();
                return JsonHelper.ConvertirAString(respuesta);
            }
        }

        [HttpPost]
        public string FiltrarParametros()
        {
            var conexion = new Conexion();
            conexion.StringConnection = cadena;
            var respuesta = new Dictionary<string, object>();
            List<Pagos> FiltroPagos = new List<Pagos>();

            try
            {
                var datos = ObtenerDatos();
                var fechaInicio = JsonHelper.ConvertirAObjeto<DateTime>(
                    JsonHelper.ConvertirAString(datos["fechaInicio"]));
                var fechaFin = JsonHelper.ConvertirAObjeto<DateTime>(
                    JsonHelper.ConvertirAString(datos["fechaFin"]));

                foreach (var pago in conexion.Listar<Pagos>())
                {
                    if (pago.Fecha >= fechaInicio && pago.Fecha <= fechaFin)
                    {
                        FiltroPagos.Add(pago);
                    }
                }
                respuesta["FechaInicio"] = fechaInicio;
                respuesta["FechaFin"] = fechaFin;
                respuesta["Entidades"] = FiltroPagos;
                return JsonHelper.ConvertirAString(respuesta);
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.Message.ToString();
                return JsonHelper.ConvertirAString(respuesta);
            }
        }

        [HttpPost]
        public string FiltrarParametrosMonto()
        {
            var conexion = new Conexion();
            conexion.StringConnection = cadena;
            var repuesta = new Dictionary<string, object>();
            List<Pagos> FiltroPagos = new List<Pagos>();

            try
            {
                var datos = ObtenerDatos();
                int monto = JsonHelper.ConvertirAObjeto<int>(
                    JsonHelper.ConvertirAString(datos["monto"]));

                foreach (var pago in conexion.Listar<Pagos>())
                {
                    if (pago.Monto == monto)
                    {
                        FiltroPagos.Add(pago);
                    }
                }
                repuesta["Monto"] = monto;
                repuesta["Entidades"] = FiltroPagos;
                return JsonHelper.ConvertirAString(repuesta);
            }
            catch (Exception ex)
            {
                repuesta["Error"] = ex.Message.ToString();
                return JsonHelper.ConvertirAString(repuesta);
            }
        }

        [HttpPost]
        public string GuardarPagoResta()
        {
            var conexion = new Conexion();
            conexion.StringConnection = cadena;
            var respuesta = new Dictionary<string, object>();

            try
            {
                var datos = ObtenerDatos();
                // toma los datos del pago
                var entidad = JsonHelper.ConvertirAObjeto<Pagos>(
                    JsonHelper.ConvertirAString(datos["Entidad"]));
                // busca la persona que paga
                var personasPagan = conexion.Buscar<Personas>(p => p.Id == entidad.IdPaga );
                var personaP = personasPagan[0];
                //le resta el saldo a la persona que paga
                personaP.Saldo-= entidad.Monto;
                //modifica la persona que paga
                conexion.Modificar(personaP);

                // busca la persona que recibe
                var personasReciben = conexion.Buscar<Personas>(p => p.Id == entidad.IdRecibe);
                var personaR = personasReciben[0];
                //le suma el saldo a la persona que recibe
                personaR.Saldo += entidad.Monto;
                //modifica la persona que recibe
                conexion.Modificar(personaR);
                //guarda el pago
                conexion.Guardar(entidad);
                //guarda los cambios
                conexion.GuardarCambios();

                respuesta["Paga"] = personaP;
                respuesta["Recibe"] = personaR;
                respuesta["Entidad"] = entidad;
                respuesta["Respuesta"] = "OK";
                respuesta["Fecha"] = DateTime.Now.ToString();
                return JsonHelper.ConvertirAString(respuesta);
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.Message.ToString();
                return JsonHelper.ConvertirAString(respuesta);
            }
        }
    }
}       
