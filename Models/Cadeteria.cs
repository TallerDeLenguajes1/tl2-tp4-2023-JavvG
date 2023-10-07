using EspacioCadete;
using EspacioPedido;
using EspacioCliente;
using EspacioAccesoADatos;

namespace Web_Api;

public class Cadeteria {

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
    public string Nombre { get => nombre; set => nombre = value; }
    public int Telefono { get => telefono; set => telefono = value; }
    public List<Cadete> ListadoCadetes { get => listadoCadetes; set => listadoCadetes = value; }
    public List<Pedido> ListadoTotalPedidos { get => listadoTotalPedidos; set => listadoTotalPedidos = value; }
    // Métodos
    public Cadeteria() {    // Constructor por defecto, inicializa una lista de cadetes para evitar errores a posteriori
        this.ListadoCadetes = new List<Cadete>();
        this.ListadoTotalPedidos = new List<Pedido>();
    }

    public List<Pedido> GetPedidos() {
        return ListadoTotalPedidos;
    }

    public List<Cadete> GetCadetes() {
        return ListadoCadetes;
    }

    public Cadeteria CargarDatos(int option) {
        Cadeteria cadeteria = null;
        switch(option) {
            case 1:
                
                AccesoCSV csv = new AccesoCSV();
                cadeteria = csv.LeerDatosCadeteria("cadeteria.csv");
            
                cadeteria.ListadoCadetes = csv.LeerDatosCadetes("cadetes.csv");
            break;
            case 2:
                AccesoJSON json = new AccesoJSON();
                cadeteria = json.LeerDatosCadeteria("cadeteria.json");
                cadeteria.ListadoCadetes = json.LeerDatosCadetes("cadetes.json");
            break;
        }
        return cadeteria;
    }

    public Pedido AgregarPedido(Pedido pedido) {

        ListadoTotalPedidos.Add(pedido);
        pedido.Numero = this.ListadoTotalPedidos.Count();
        return pedido;

    }

    public string CrearPedido(int numeroPedido) {
        Console.WriteLine("\n - NUEVO PEDIDO -");
        Console.Write("\n - Descripción del pedido: ");
        string observacion = Console.ReadLine();
        while(string.IsNullOrEmpty(observacion.Trim())) {
            Console.Write("\n - (!) Ingrese una descripción válida: ");
            observacion = Console.ReadLine();
        }
        Console.Write("\n - Nombre del cliente: ");
        string nombreCliente = Console.ReadLine();
        while(string.IsNullOrEmpty(nombreCliente.Trim())) {
            Console.Write("\n (!) Ingrese un nombre válido: ");
            nombreCliente = Console.ReadLine();
        }
        Console.Write("\n - Dirección del cliente: ");
        string direccionCliente = Console.ReadLine();
        while(string.IsNullOrEmpty(direccionCliente.Trim())) {
            Console.Write("\n (!) Ingrese una dirección válida: ");
            direccionCliente = Console.ReadLine();
        }
        Console.Write("\n - Número de teléfono del cliente: ");
        string input = Console.ReadLine();
        long telefonoCliente;
        while(!long.TryParse(input, out telefonoCliente)) {
            Console.Write("\n (!) Ingrese un número válido: ");
            input = Console.ReadLine();
        }
        Console.Write("\n - Monto del pedido: $");
        input = Console.ReadLine();
        double montoPedido;
        while(!double.TryParse(input, out montoPedido)) {
            Console.Write("\n (!) Ingrese un valor válido: ");
            input = Console.ReadLine();
        }
        
        Cliente nuevoCliente = new Cliente(nombreCliente, direccionCliente, telefonoCliente);       // Nueva intancia para cliente
        Pedido nuevoPedido = new Pedido(numeroPedido, observacion, nuevoCliente, EstadoPedido.Pendiente, false, montoPedido);      // Nueva instancia para el pedido
        this.listadoTotalPedidos.Add(nuevoPedido);
        nuevoPedido.VerDatosPedido();
        return("\n\n >> El pedido ha sido creado exitosamente. \n");
    }
    public Cadete AsignarPedido(int numeroPedido, int IdCadete) {

        Pedido pedidoSeleccionado = this.ListadoTotalPedidos.Find(pedido => pedido.Numero == numeroPedido);
        Cadete cadeteSeleccionado = this.ListadoCadetes.Find(cadete => cadete.IdCadete == IdCadete);
        
        pedidoSeleccionado.Asignado = true;
        cadeteSeleccionado.ListadoPedidos.Add(pedidoSeleccionado);

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

        return pedidoAModificar;

    }
    public Cadete SeleccionarCadetePorID() {
        int idCadeteBuscado;
        Console.Write("\n > Ingrese el ID del cadete que desea seleccionar: ");
        string input = Console.ReadLine();
        while(!int.TryParse(input, out idCadeteBuscado)) {
            Console.Write("\n\n (!) Ha ingresado una opción inválida.\n > Ingrese nuevamente: ");
            input = Console.ReadLine();
        }
        Cadete cadeteSeleccionado = this.ListadoCadetes.Find(cadete => cadete.IdCadete == idCadeteBuscado);
        while(cadeteSeleccionado == null) {
            Console.Write($"\n\n (!) No se ha encontrado el cadete con ID {idCadeteBuscado}.\n > Ingrese un nuevo valor: ");
            
            input = Console.ReadLine();
            while(!int.TryParse(input, out idCadeteBuscado)) {
                Console.Write("\n\n (!) Ha ingresado una opción inválida.\n > Ingrese nuevamente: ");
                input = Console.ReadLine();
            }
            cadeteSeleccionado = this.ListadoCadetes.Find(cadete => cadete.IdCadete == idCadeteBuscado);
        }
        return cadeteSeleccionado;
    }

    public Cadete ReasignarPedido(int numeroPedido,int idNuevoCadete) {

        Pedido pedidoSeleccionado = this.ListadoTotalPedidos.Find(pedido => pedido.Numero == numeroPedido);
        
        foreach(Cadete C in this.ListadoCadetes) {
            C.ListadoPedidos.RemoveAll(pedido => pedido.Numero == numeroPedido);
        }

        Cadete cadeteAsignado = this.ListadoCadetes.Find(cadete => cadete.IdCadete == idNuevoCadete);

        cadeteAsignado.ListadoPedidos.Add(pedidoSeleccionado);

        return cadeteAsignado;

    }
    public string ReasignarPedidox() {
        Console.WriteLine("\n - REASIGNACIÓN DE PEDIDO - ");
        if(this.ListadoTotalPedidos.Count() > 0) {
            Console.WriteLine("\n - ELECCIÓN DEL PEDIDO A REASIGNAR:  ");        // Se muestran los pedidos que no tienen un cadete asignado
            foreach(Pedido P in this.ListadoTotalPedidos) {
                if(P.Asignado == true) {
                    P.VerDatosPedido();
                }
            }
            Pedido pedidoAReasignar = this.SeleccionarPedidoAsignado();     // Se elige el pedido a reasignar de una lista de pedidos con asignacion de cadete afirmativa
            Console.Clear();
            foreach(Cadete C in this.ListadoCadetes) {
                Pedido pedidoAEliminar = C.ListadoPedidos.Find(pedido => pedido == pedidoAReasignar);
                if(pedidoAEliminar != null) {       // Si el pedido a reasignar se encuentra en alguna de las listas de pedidos de algun cadete, se elimina
                    C.EliminarPedido(pedidoAEliminar);
                }
            }
            Console.WriteLine("\n - ELECCIÓN DEL CADETE AL QUE DESEA REASIGNAR EL PEDIDO: ");
            foreach(Cadete C in this.ListadoCadetes) {
                C.VerDatosCadete();
            }
            Cadete cadeteSeleccionado = this.SeleccionarCadetePorID();
            cadeteSeleccionado.ListadoPedidos.Add(pedidoAReasignar);
            Console.Clear();
            cadeteSeleccionado.VerDatosCadete();
            return("\n >> Pedido reasignado exitosamente");
        }
        else {
            return("\n (!) No hay pedidos registrados para ser reasignados");
        }
        
    }
    public Pedido SeleccionarPedidoAsignado() {
        string input;
        int numeroPedidoBuscado;
        Console.Write("\n > Ingrese el número del pedido que desea seleccionar: ");
        input = Console.ReadLine();
        while(!int.TryParse(input, out numeroPedidoBuscado)) {
            Console.Write("\n\n (!) Ha ingresado una opción inválida.\n > Ingrese nuevamente: ");
            input = Console.ReadLine();
        }
        Pedido pedidoSeleccionado = this.ListadoTotalPedidos.Find(pedido => pedido.Numero == numeroPedidoBuscado && pedido.Asignado == true);
        while(pedidoSeleccionado == null) {
            Console.Write($"\n\n (!) No se ha encontrado el pedido número {numeroPedidoBuscado}.\n > Ingrese un nuevo valor: ");
            
            input = Console.ReadLine();
            while(!int.TryParse(input, out numeroPedidoBuscado)) {
                Console.Write("\n\n (!) Ha ingresado una opción inválida.\n > Ingrese nuevamente: ");
                input = Console.ReadLine();
            }
            pedidoSeleccionado = this.ListadoTotalPedidos.Find(pedido => pedido.Numero == numeroPedidoBuscado);
        }
        return pedidoSeleccionado;
    }
    public int CantidadDeCadetes() {
        return this.ListadoCadetes.Count();
    }
    public int CantidadDePedidos() {
        return this.ListadoTotalPedidos.Count();
    }
    public double RecaudacionTotal() {
        double recaudacion = 0;
        var listadoPedidosNoCancelados = this.ListadoTotalPedidos.Where(pedido => pedido.Estado != EstadoPedido.Cancelado);
        foreach(Pedido P in listadoPedidosNoCancelados) {
            recaudacion += P.Monto;
        }
        return recaudacion;
    }
}