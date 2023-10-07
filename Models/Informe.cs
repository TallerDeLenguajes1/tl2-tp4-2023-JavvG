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
        ListaCadetes = null;
        ListaPedidos = null;
    }

    public Informe(List<Pedido> pedidos, List<Cadete> cadetes, double recaudacion) {

        ListaPedidos = pedidos;
        PedidosPendientes = (List<Pedido>)ListaPedidos.Where(pedido => pedido.Estado == EstadoPedido.Pendiente);
        PedidosEntregados = (List<Pedido>)ListaPedidos.Where(pedido => pedido.Estado == EstadoPedido.Entregado);
        PedidosCancelados = (List<Pedido>)ListaPedidos.Where(pedido => pedido.Estado == EstadoPedido.Cancelado);

        ListaCadetes = cadetes;
        Recaudacion = recaudacion;
        PedidosTotales = ListaPedidos.Count();

    }
}