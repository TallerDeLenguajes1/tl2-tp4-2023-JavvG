using Microsoft.AspNetCore.Mvc;     // Espacio de nombre donde está ControllerBase
using EspacioPedido;
using EspacioCadete;
using EspacioInforme;

namespace Web_Api.Controllers;       // Espacio de nombre 

[ApiController]     // Atributo que indica que es un controlador
[Route("[controller]")]     // Atributo que indica la ruta con la que se va a direccionar el recurso, en este caso se utilizará el nombre de la clase

public class CadeteriaController : ControllerBase {     // Clase que hereda de ControllerBase

    // Se usa el patrón "Singleton": un patrón de diseño que se utiliza para garantizar que una clase 
    // tenga una única instancia y proporcionar un punto de acceso global a esa instancia desde cualquier 
    //parte de la aplicación.

    // En este caso, para usar Singleton se modifica la clase Cadeteria para que no puedan crearse instancias desde fuera 
    // de la clase, sino que sólo pueda accederse a una instancia ya inicada usando un método

    private Cadeteria cadeteria;     // Siguiendo el patrón Singleton, se crea una instancia privada 

    private readonly ILogger<CadeteriaController> _logger;

    bool uploadedData = false;      // booleano que indica si los datos fueron cargados correctamente

    public CadeteriaController(ILogger<CadeteriaController> logger) {
        _logger = logger;
        cadeteria = Cadeteria.GetInstanciaCadeteria();
        if(cadeteria == null) {     // Si los datos se cargaron correctamente, el booleano "uploadedData" lo indica
            uploadedData = false;
        }
        else {
            uploadedData = true;
        }
    }

    // Endpoints

    // "IActionResult" y "ActionResult<T>" : es una interfaz comúnmente utilizada para
    // representar el resultado de una acción en un controlador ASP.NET Core. Esta
    // interfaz permite que una acción devuelva una variedad de tipos de resultados,
    // lo que proporciona flexibilidad al desarrollador para elegir el tipo de respuesta
    // que mejor se adapte a la situación.

    [HttpGet("InformacionCadeteria")]
    public ActionResult<string> GetInfo(){

        if(uploadedData){

            string informacion = $" Nombre: {cadeteria.Nombre} \n Teléfono: {cadeteria.Telefono}";
            return Ok(informacion);

        } else{
            return NotFound(" (!) La información no se ha cargado correctamente. ");
        }

    }

    // En la mayoría de los casos, es preferible utilizar "IEnumerable<Pedido>" en lugar de "List<Pedido>"
    // en los controladores de ASP.NET Core, ya que proporciona más flexibilidad y evita acoplar tu API a una 
    // implementación de lista específica

    [HttpGet("Pedidos")]
    public ActionResult<IEnumerable<Pedido>> GetPedidos(){
        
        if(uploadedData) {

            var lista = cadeteria.GetPedidos();

            if(lista.Count() > 0){
                return Ok(lista);
            }
            else{
                return NotFound(" (!) El recurso no se ha encontrado.");
            }

        }
        else {
            return NotFound(" (!) La información no se ha cargado correctamente. ");
        }

    }

    [HttpGet("Cadetes")]
    public ActionResult<IEnumerable<Cadete>> GetCadetes() {
        
        if(uploadedData) {

            var cadetes = cadeteria.GetCadetes();
        
            if(cadetes.Count() > 0) {
                return Ok(cadetes);
            }
            else {
                return NotFound(" (!) El recurso no se ha encontrado.");
            }

        }
        return NotFound(" (!) La información no se ha cargado correctamente. ");

    }

    [HttpGet("Informe")]
    public ActionResult<Informe> GetInforme(){

        if((cadeteria.ListadoTotalPedidos.Count() > 0) && (cadeteria.ListadoCadetes.Count() > 0)) {

            if(uploadedData){
                return Ok(cadeteria.GetInforme());
            } else{
                return NotFound(" (!) No ha podido generarse el informe. Los datos no han sido cargados correctamente");
            }

        }
        else {
            return BadRequest(" (!) No ha podido generarse el informe debido a falta de datos en la cadeteria (pedidos y/o cadetes).");
        }

    }
    
    [HttpPost("NuevoPedido")]
    public ActionResult<Pedido> AddPedido(string observaciones, string nombreCliente, string direccionCliente, long telefonoCliente, double monto)  {

        var nuevoPedido = cadeteria.AddPedido(observaciones, nombreCliente, direccionCliente, telefonoCliente, monto);

        if(nuevoPedido != null) {
            return Ok(nuevoPedido);
        }
        else {
            return BadRequest(" (!) No ha podido guardare el nuevo pedido.");
        }

    }
    
    [HttpPut("AsignarPedido")]
    public ActionResult<Cadete> AsignarPedido(int numeroPedido, int IdCadete) {

        if((cadeteria.ListadoCadetes.Find(cadete => cadete.IdCadete == IdCadete)) != null && (cadeteria.ListadoTotalPedidos.Find(pedido => pedido.Numero == numeroPedido)) != null && ((cadeteria.ListadoTotalPedidos.Find(pedido => pedido.Numero == numeroPedido)).Asignado == false)) {

            var cadeteAsignado = cadeteria.AsignarPedido(numeroPedido, IdCadete);

            if(cadeteAsignado != null) {
                return Ok(cadeteAsignado);
            }
            else {
                return BadRequest(" (!) No ha podido completarse la asignación.");
            }

        }
        else {
            return BadRequest(" (!) El cadete o el pedido no existen, o el pedido ya está asignado a un cadete.");
        }
        
    }

    [HttpPut("CambiarEstadoPedido")]
    public ActionResult<Pedido> CambiarEstadoPedido(int numeroPedido, int nuevoEstado) {

        if(cadeteria.ListadoTotalPedidos.Find(pedido => pedido.Numero == numeroPedido) != null) {

            var pedidoAModificar = cadeteria.CambiarEstadoPedido(numeroPedido, nuevoEstado);

            if(pedidoAModificar != null) {
                return Ok(pedidoAModificar);
            }
            else {
                return BadRequest(" (!) No ha podido cambiarse el estado del pedido.");
            }

        }
        else {
            return BadRequest(" (!) El pedido no existe.");  
        }
        
    }

    [HttpPut("ReasignarPedido")]
    public ActionResult<Pedido> ReasignarPedido(int numeroPedido,int idNuevoCadete) {

        if((cadeteria.ListadoCadetes.Find(cadete => cadete.IdCadete == idNuevoCadete)) != null && (cadeteria.ListadoTotalPedidos.Find(pedido => pedido.Numero == numeroPedido)) != null) {

            var pedidoAModificar = cadeteria.ReasignarPedido(numeroPedido, idNuevoCadete);

            if(pedidoAModificar != null) {
                return Ok(pedidoAModificar);
            }
            else {
                return BadRequest(" (!) No ha podido reasignarse el pedido.");
            }

        }
        else {
            return BadRequest(" (!) El cadete o el pedido no existen.");
        }
    }

    [HttpPost("NuevoCadete")]
    public ActionResult<Cadete> AddCadete(string nombre, string direccion, long telefono) {

        var nuevoCadete = cadeteria.AddCadete(nombre, direccion, telefono);

        if(nuevoCadete != null) {
            return Ok(nuevoCadete);
        }
        else {
            return BadRequest(" (!) No ha podido añadirse el nuevo cadete.");
        }
        
    }

    [HttpGet("ObtenerPedido/{numeroPedido}")]
    public ActionResult<Pedido> GetPedido(int numeroPedido) {

        if(uploadedData) {

            if(cadeteria.ListadoTotalPedidos.Count() > 0) {

                var pedido = cadeteria.GetPedido(numeroPedido);

                if(pedido != null) {
                    return Ok(pedido);
                }
                else {
                    return BadRequest(" (!) No ha podido encontrarse el pedido.");
                }

            }
            else {
                return BadRequest(" (!) No hay pedidos registrados.");
            }
            

        }
        else {
            return NotFound(" (!) La información no se ha cargado correctamente. ");
        }

    }

    [HttpGet("ObtenerCadete/{idCadete}")]
    public ActionResult<Cadete> GetCadete(int idCadete) {

        if(uploadedData) {

            if(cadeteria.ListadoCadetes.Count() > 0) {

                var cadete = cadeteria.GetCadete(idCadete);

                if(cadete != null) {
                    return Ok(cadete);
                }
                else {
                    return BadRequest(" (!) No ha podido encontrarse el cadete.");
                }

            }
            else {
                return BadRequest(" (!) No hay cadetes registrados.");
            }

        }
        else {
            return NotFound(" (!) La información no se ha cargado correctamente. ");
        }

    }

}