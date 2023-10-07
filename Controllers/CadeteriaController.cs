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

    public CadeteriaController(ILogger<CadeteriaController> logger) {
        _logger = logger;
        cadeteria = cadeteria.GetCadeteria();
    }

    // Endpoints

    // "IActionResult" y "ActionResult<T>" : es una interfaz comúnmente utilizada para
    // representar el resultado de una acción en un controlador ASP.NET Core. Esta
    // interfaz permite que una acción devuelva una variedad de tipos de resultados,
    // lo que proporciona flexibilidad al desarrollador para elegir el tipo de respuesta
    // que mejor se adapte a la situación.

    [HttpGet("Pedidos")]
    public ActionResult<List<Pedido>> GetPedidos() {
        var pedidos = cadeteria.GetPedidos();
        return Ok(pedidos);
    }

    [HttpGet("Informe")]
    public ActionResult<Informe> GetInforme() {
        var informe = cadeteria.GetInforme();
        return Ok(informe);
    } 

    [HttpGet("Cadetes")]
    public ActionResult<List<Cadete>> GetCadetes() {
        var cadetes = cadeteria.GetCadetes();
        return Ok(cadetes);
    }

    [HttpPost("NuevoPedido")]
    public ActionResult<Pedido> AgregarPedido(Pedido pedido) {
        var nuevoPedido = cadeteria.AgregarPedido(pedido);
        return Ok(nuevoPedido);
    }

    [HttpPut("AsignarPedido")]
    public ActionResult<Cadete> AsignarPedido(int numeroPedido, int IdCadete) {
        var cadeteAsignado = cadeteria.AsignarPedido(numeroPedido, IdCadete);
        return Ok(cadeteAsignado);
    }

    [HttpPut("CambiarEstadoPedido")]
    public ActionResult<Pedido> CambiarEstadoPedido(int numeroPedido, int nuevoEstado) {
        var pedidoAModificar = cadeteria.CambiarEstadoPedido(numeroPedido, nuevoEstado);
        return Ok(pedidoAModificar);
    }

    [HttpPut("ReasignarPedido")]
    public ActionResult<Cadete> ReasignarPedido(int numeroPedido,int idNuevoCadete) {
        var nuevoCadete = cadeteria.ReasignarPedido(numeroPedido, idNuevoCadete);
        return Ok(nuevoCadete);
    }

}