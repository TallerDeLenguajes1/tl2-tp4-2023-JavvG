using EspacioPedido;
using EspacioCadete;

namespace EspacioInforme;

public class Informe {

    private List<Cadete> listaCadetes;
    private List<Pedido> listaPedidos;
    private List<Pedido> pedidosPendientes;
    private List<Pedido> pedidosEntregados;
    private List<Pedido> pedidosCancelados;
    int pedidosTotales;
    private double recaudacion;

    public List<Cadete> ListaCadetes { get => listaCadetes; set => listaCadetes = value; }
    public List<Pedido> ListaPedidos { get => listaPedidos; set => listaPedidos = value; }
    public double Recaudacion { get => recaudacion; set => recaudacion = value; }
    public List<Pedido> PedidosPendientes { get => pedidosPendientes; set => pedidosPendientes = value; }
    public List<Pedido> PedidosEntregados { get => pedidosEntregados; set => pedidosEntregados = value; }
    public List<Pedido> PedidosCancelados { get => pedidosCancelados; set => pedidosCancelados = value; }
    public int PedidosTotales { get => pedidosTotales; set => pedidosTotales = value; }

    public Informe() {
        ListaCadetes = new List<Cadete>();
        ListaPedidos = new List<Pedido>();
    }

    public Informe(List<Pedido> pedidos, List<Cadete> cadetes, List<Pedido> pedidosPendientes, List<Pedido> pedidosEntregados, List<Pedido> pedidosCancelados,  double recaudacion) {

        this.ListaPedidos = pedidos;
        this.PedidosPendientes = pedidosPendientes;
        this.PedidosEntregados = pedidosEntregados;
        this.PedidosCancelados = pedidosCancelados;
        this.ListaCadetes = cadetes;
        this.Recaudacion = recaudacion;
        this.PedidosTotales = ListaPedidos.Count();

    }
}