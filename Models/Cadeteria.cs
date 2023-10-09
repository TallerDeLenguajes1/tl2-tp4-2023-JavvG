using EspacioCadete;
using EspacioPedido;
using EspacioCliente;
using DatosPedidos;
using DatosCadetes;
using DatosCadeteria;
using EspacioInforme;

namespace Web_Api;

public class Cadeteria {

    private AccesoADatosCadetes accesoDatosCadetes;
    private AccesoADatosPedidos accesoDatosPedidos;

    // Modificación siguiendo el patrón Singleton

    private static Cadeteria cadeteriaInstancia;     // Se crea una instancia estática y privada (*)

    private string nombre;
    private int telefono;
    private List<Cadete> listadoCadetes;
    private List<Pedido> listadoTotalPedidos;

    public string Nombre { get => nombre; set => nombre = value; }
    public int Telefono { get => telefono; set => telefono = value; }
    public List<Cadete> ListadoCadetes { get => listadoCadetes; set => listadoCadetes = value; }
    public List<Pedido> ListadoTotalPedidos { get => listadoTotalPedidos; set => listadoTotalPedidos = value; }
    public AccesoADatosCadetes AccesoDatosCadetes { get => accesoDatosCadetes; set => accesoDatosCadetes = value; }
    public AccesoADatosPedidos AccesoDatosPedidos { get => accesoDatosPedidos; set => accesoDatosPedidos = value; }

    // Métodos

    //(*)
    //La única forma de obtener una instancia de "Cadeteria" es a través 
    // del método estático público "GetCadeteria()", que crea la instancia si aún no existe 
    // o devuelve la instancia existente si ya se ha creado. Esto asegura que siempre haya 
    // una única instancia de "Cadeteria" en la aplicación.

    /* public static Cadeteria GetCadeteria() {

        // Verifica si ya existe una instancia de "Cadeteria"
        if (cadeteriaInstancia == null) {

            // Si no existe, crea una instancia de "AccesoADatosCadeteria"
            var datosCadeteria = new AccesoADatosCadeteria();

            // Verifica si se pueden obtener datos de la cadetería
            if (datosCadeteria.Obtener() != null) {

                // Si se pueden obtener datos, asigna la instancia de Cadeteria a cadeteriaInstancia
                cadeteriaInstancia = datosCadeteria.Obtener();

                // Inicializa las propiedades "AccesoDatosCadetes" y "AccesoDatosPedidos" de la instancia de Cadeteria
                cadeteriaInstancia.AccesoDatosCadetes = new AccesoADatosCadetes();
                cadeteriaInstancia.AccesoDatosPedidos = new AccesoADatosPedidos();

            }
        }

        // Devuelve la instancia de "Cadeteria" (ya sea cargada con datos o vacía)
        return cadeteriaInstancia;
    } */

    public static Cadeteria GetInstanciaCadeteria() {

        if(cadeteriaInstancia == null) {
            cadeteriaInstancia = new Cadeteria("\0", 0);
        }
        return cadeteriaInstancia;
    }
    
    public Cadeteria() {}

    public Cadeteria(string nombre, int telefono){

        this.Nombre = nombre;
        this.Telefono = telefono;
        this.ListadoCadetes = new();
        this.ListadoTotalPedidos = new();
        this.AccesoDatosCadetes = new();
        this.AccesoDatosPedidos = new();

        AccesoADatosCadeteria datosCadeteria = new AccesoADatosCadeteria();

        Cadeteria cadAux = datosCadeteria.Obtener();
        
        if(cadAux != null) {
            this.Nombre = cadAux.Nombre;
            this.Telefono = cadAux.Telefono;
        }

        List<Pedido> lista1 = AccesoDatosPedidos.Obtener();
        List<Cadete> lista2 = AccesoDatosCadetes.Obtener();

        if(lista1 != null) {
            this.ListadoTotalPedidos = lista1;
        }

        if(lista2 != null) {
            this.ListadoCadetes = lista2;
        }
        
    } 

    public List<Pedido> GetPedidos() {
        return ListadoTotalPedidos;
    }

    public List<Cadete> GetCadetes() {
        return ListadoCadetes;
    }

    public Pedido AddPedido(string observaciones, string nombreCliente, string direccionCliente, long telefonoCliente, double monto) {

        Cliente nuevoCliente = this.AddCliente(nombreCliente, direccionCliente, telefonoCliente);
        Pedido nuevoPedido = new Pedido(this.ListadoTotalPedidos.Count() + 1, observaciones, nuevoCliente, EstadoPedido.Pendiente, false, monto);
        
        this.ListadoTotalPedidos.Add(nuevoPedido);

        return nuevoPedido;

    }

    public Cliente AddCliente(string nombre, string direccion, long telefono) {

        Cliente nuevoCliente = new Cliente(nombre, direccion, telefono);
        return nuevoCliente;
    }

    public Cadete AddCadete(string nombre, string direccion, long telefono) {

        Cadete nuevoCadete = new();
        nuevoCadete.IdCadete = this.ListadoCadetes.Count() + 1;
        nuevoCadete.Nombre = nombre;
        nuevoCadete.Direccion = direccion;
        nuevoCadete.Telefono = telefono;
        nuevoCadete.ListadoPedidos = new();

        this.ListadoCadetes.Add(nuevoCadete);

        this.AccesoDatosCadetes.Guardar(this.ListadoCadetes);

        return nuevoCadete;
    }

    public Cadete AsignarPedido(int numeroPedido, int IdCadete) {
        
        Pedido pedidoSeleccionado = this.ListadoTotalPedidos.Find(pedido => pedido.Numero == numeroPedido);
        Cadete cadeteSeleccionado = this.ListadoCadetes.Find(cadete => cadete.IdCadete == IdCadete);

        foreach(Cadete C in this.ListadoCadetes) {
            C.ListadoPedidos.RemoveAll(pedido => pedido == pedidoSeleccionado);
        }
        
        pedidoSeleccionado.Asignado = true;
        cadeteSeleccionado.ListadoPedidos.Add(pedidoSeleccionado);

        AccesoDatosPedidos.Guardar(ListadoTotalPedidos);

        return cadeteSeleccionado;

    }
    
    public Pedido CambiarEstadoPedido(int numeroPedido, int nuevoEstado) {
        
        Pedido pedidoAModificar = this.ListadoTotalPedidos.Find(pedido => pedido.Numero == numeroPedido);
        
        switch(nuevoEstado) {
            case 0:
                pedidoAModificar.Estado = EstadoPedido.Pendiente;
            break;
            case 1:
                pedidoAModificar.Estado = EstadoPedido.EnPreparacion;
            break;
            case 2:
                pedidoAModificar.Estado = EstadoPedido.AsignadoACadete;
            break;
            case 3:
                pedidoAModificar.Estado = EstadoPedido.EnCamino;
            break;
            case 4:
                pedidoAModificar.Estado = EstadoPedido.Entregado;
            break;
            case 5:
                pedidoAModificar.Estado = EstadoPedido.Cancelado;
            break;
            default:
                pedidoAModificar.Estado = pedidoAModificar.Estado;
            break;
        }

        AccesoDatosPedidos.Guardar(ListadoTotalPedidos);

        return pedidoAModificar;

    }

    public Pedido ReasignarPedido(int numeroPedido,int idNuevoCadete) {

        Pedido pedidoSeleccionado = this.ListadoTotalPedidos.Find(pedido => pedido.Numero == numeroPedido);
        Cadete cadeteSeleccionado = this.ListadoCadetes.Find(cadete => cadete.IdCadete == idNuevoCadete);

        cadeteSeleccionado.ListadoPedidos.Add(pedidoSeleccionado);
        pedidoSeleccionado.Asignado = true;

        AccesoDatosPedidos.Guardar(ListadoTotalPedidos);

        return pedidoSeleccionado;

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

        List<Cadete> listaCad = this.ListadoCadetes;
        List<Pedido> listaPed = this.ListadoTotalPedidos;
        List<Pedido> pedidosPendientes = new();
        List<Pedido> pedidosEntregados = new();
        List<Pedido> pedidosCancelados = new();
        double recaudacion = this.RecaudacionTotal();
        
        foreach(Pedido P in this.ListadoTotalPedidos) {

            if(P.Estado == EstadoPedido.Pendiente) {
                pedidosPendientes.Add(P);
            }

            if(P.Estado == EstadoPedido.Entregado) {
                pedidosEntregados.Add(P);
            }

            if(P.Estado == EstadoPedido.Cancelado) {
                pedidosCancelados.Add(P);
            }

        }

        Informe nuevoInforme = new Informe(listaPed, listaCad, pedidosPendientes, pedidosEntregados, pedidosCancelados, recaudacion);
        return nuevoInforme;
    }

    public Pedido GetPedido(int numeroPedido) {
        return this.ListadoTotalPedidos.Find(pedido => pedido.Numero == numeroPedido);
    }

    public Cadete GetCadete(int idCadete) {
        return this.ListadoCadetes.Find(cadete => cadete.IdCadete == idCadete);
    }

}