using EspacioPedido;

namespace EspacioCadete {

    public class Cadete {

        private int idCadete;
        private string nombre;
        private string direccion;
        private long telefono;
        private List<Pedido> listadoPedidos;

        public int IdCadete { get => idCadete; set => idCadete = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Direccion { get => direccion; set => direccion = value; }
        public long Telefono { get => telefono; set => telefono = value; }
        public List<Pedido> ListadoPedidos { get => listadoPedidos; set => listadoPedidos = value; }

        public Cadete() {       // Constructor por defecto, inicializa la lista de pedidos
            this.listadoPedidos = new List<Pedido>();
        }

        public void VerDatosCadete() {

            Console.WriteLine($"\n\n CADETE ID: {this.IdCadete}");
            Console.WriteLine($" Nombre: {this.Nombre}");
            Console.WriteLine($" Dirección: {this.Direccion}");
            Console.WriteLine($" Teléfono: {this.Telefono}\n");

            if(this.ListadoPedidos.Count() > 0) {
                Console.Write(" Pedidos asignados: \n");
                foreach(Pedido P in this.ListadoPedidos) {
                    P.VerDatosPedido();
                }
            }
            else {
                Console.WriteLine(" (!) El cadete no tiene pedidos asignados\n\n");
            }

        }

        public double JornalACobrar() {
            return this.ListadoPedidos.Count() * 500;       // Comisión de 500 ARS por pedido
        }

        public int CantidadDePedidos() {
            return this.ListadoPedidos.Count();
        }

        public void AgregarPedido(Pedido nuevoPedido) {
            this.ListadoPedidos.Add(nuevoPedido);
        }

        public void EliminarPedido(Pedido pedidoAEliminar) {
            this.ListadoPedidos.RemoveAll(pedido => pedido == pedidoAEliminar);
        }

    }

}