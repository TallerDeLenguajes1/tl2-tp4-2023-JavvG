using EspacioCadete;
using EspacioPedido;
using EspacioCliente;
using DatosPedidos;
using EspacioInforme;

namespace Web_Api;

public class Cadeteria {

    private AccesoADatosPedidos accesoDatosPedidos;      // Atributo private que indica que puede haber algún tipo de relación entre las clases "Cadetería" y "AccesoADatosPedidos"

    // Modificación siguiendo el patrón Singleton
    private static Cadeteria cadeteriaInstancia;     // Se crea una instancia estática y privada
    //La única forma de obtener una instancia de "Cadeteria" es a través 
    // del método público "GetCadeteria()", que crea la instancia si aún no existe 
    // o devuelve la instancia existente si ya se ha creado. Esto asegura que siempre haya 
    // una única instancia de "Cadeteria" en la aplicación.
    public Cadeteria GetCadeteria() {      
        
        if(cadeteriaInstancia == null) {
            cadeteriaInstancia = new Cadeteria();
            return cadeteriaInstancia;
        }
        else {
            return cadeteriaInstancia;
        }
    }
    private string nombre;
    private int telefono;
    private List<Cadete> listadoCadetes;
    private List<Pedido> listadoTotalPedidos;
    private Informe informeCadeteria;

    public string Nombre { get => nombre; set => nombre = value; }
    public int Telefono { get => telefono; set => telefono = value; }
    public List<Cadete> ListadoCadetes { get => listadoCadetes; set => listadoCadetes = value; }
    public List<Pedido> ListadoTotalPedidos { get => listadoTotalPedidos; set => listadoTotalPedidos = value; }
    public Informe InformeCadeteria { get => informeCadeteria; set => informeCadeteria = value; }
    public AccesoADatosPedidos AccesoDatosPedidos { get => accesoDatosPedidos; set => accesoDatosPedidos = value; }


    // Métodos
    public Cadeteria() {    // Constructor por defecto, inicializa una lista de cadetes para evitar errores a posteriori
        this.ListadoCadetes = new List<Cadete>();
        this.ListadoTotalPedidos = new List<Pedido>();
        this.InformeCadeteria = this.GetInforme();
        this.AccesoDatosPedidos = new AccesoADatosPedidos();     // Se crea una relación de composición entre ambas clases, siempre y cuando la instancia de "AccesoADatosPedidos" se cree y gestione exclusivamente dentro del constructor de la clase "Cadeteria" y no esté disponible de forma independiente fuera de la "Cadeteria".
    }

    public List<Pedido> GetPedidos() {
        return ListadoTotalPedidos;
    }

    public List<Cadete> GetCadetes() {
        return ListadoCadetes;
    }

    public Pedido AgregarPedido(Pedido pedido) {

        ListadoTotalPedidos.Add(pedido);
        pedido.Numero = this.ListadoTotalPedidos.Count();
        AccesoDatosPedidos.Guardar(ListadoTotalPedidos);
        return pedido;

    }

    public Cadete AsignarPedido(int numeroPedido, int IdCadete) {
        
        Pedido pedidoSeleccionado = this.ListadoTotalPedidos.Find(pedido => pedido.Numero == numeroPedido);
        Cadete cadeteSeleccionado = this.ListadoCadetes.Find(cadete => cadete.IdCadete == IdCadete);
        
        pedidoSeleccionado.Asignado = true;
        pedidoSeleccionado.CadeteAsignado = cadeteSeleccionado;
        cadeteSeleccionado.ListadoPedidos.Add(pedidoSeleccionado);

        AccesoDatosPedidos.Guardar(ListadoTotalPedidos);

        return cadeteSeleccionado;

    }
    
    public Pedido CambiarEstadoPedido(int numeroPedido, int nuevoEstado) {
        
        Pedido pedidoAModificar = this.ListadoTotalPedidos.Find(pedido => pedido.Numero == numeroPedido);
        
        switch(nuevoEstado) {
            case 1:
                pedidoAModificar.Estado = EstadoPedido.Pendiente;
            break;
            case 2:
                pedidoAModificar.Estado = EstadoPedido.EnPreparacion;
            break;
            case 3:
                pedidoAModificar.Estado = EstadoPedido.AsignadoACadete;
            break;
            case 4:
                pedidoAModificar.Estado = EstadoPedido.EnCamino;
            break;

            case 5:
                pedidoAModificar.Estado = EstadoPedido.Entregado;
            break;
            case 6:
                pedidoAModificar.Estado = EstadoPedido.Cancelado;
            break;
            default:
                pedidoAModificar.Estado = pedidoAModificar.Estado;
            break;
        }

        AccesoDatosPedidos.Guardar(ListadoTotalPedidos);

        return pedidoAModificar;

    }

    public Cadete ReasignarPedido(int numeroPedido,int idNuevoCadete) {

        Pedido pedidoSeleccionado = this.ListadoTotalPedidos.Find(pedido => pedido.Numero == numeroPedido);
        
        foreach(Cadete C in this.ListadoCadetes) {
            C.ListadoPedidos.RemoveAll(pedido => pedido.Numero == numeroPedido);
        }

        Cadete cadeteAsignado = this.ListadoCadetes.Find(cadete => cadete.IdCadete == idNuevoCadete);

        cadeteAsignado.ListadoPedidos.Add(pedidoSeleccionado);
        pedidoSeleccionado.CadeteAsignado = cadeteAsignado;
        AccesoDatosPedidos.Guardar(ListadoTotalPedidos);
        return cadeteAsignado;

    }
    
    public double RecaudacionTotal() {
        double recaudacion = 0;
        var listadoPedidosNoCancelados = this.ListadoTotalPedidos.Where(pedido => pedido.Estado != EstadoPedido.Cancelado);
        foreach(Pedido P in listadoPedidosNoCancelados) {
            recaudacion += P.Monto;
        }
        return recaudacion;
    }

    public Informe GetInforme() {
        Informe nuevoInforme = new Informe(this.ListadoTotalPedidos, this.ListadoCadetes, this.RecaudacionTotal());
        return nuevoInforme;
    }
}