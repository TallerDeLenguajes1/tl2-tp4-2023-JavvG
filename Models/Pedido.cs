using EspacioCliente;

namespace EspacioPedido {
    
    public enum EstadoPedido {
        Pendiente,
        EnPreparacion,
        AsignadoACadete,
        EnCamino,
        Entregado,
        Cancelado
    }

    public class Pedido {

        private int numero;
        private string observaciones;
        private Cliente cliente;
        private EstadoPedido estado;
        private double monto;
        private bool asignado;

        public int Numero { get => numero; set => numero = value; }
        public string Observaciones { get => observaciones; set => observaciones = value; }
        public Cliente Cliente { get => cliente; set => cliente = value; }
        public EstadoPedido Estado { get => estado; set => estado = value; }
        public bool Asignado { get => asignado; set => asignado = value; }
        public double Monto { get => monto; set => monto = value; }

        public Pedido() {       // Cada vez que se crea un pedido, se crea en estado "Pendiente"
            this.Estado = EstadoPedido.Pendiente;
            this.Asignado = false;
        }

        public Pedido(int numero, string observaciones, Cliente cliente, EstadoPedido estado, bool asignado, double monto) {
            this.Numero = numero;
            this.Observaciones = observaciones;
            this.Cliente = cliente;
            this.Estado = estado;
            this.Asignado = asignado;
            this.Monto = monto;
        }

        public void VerDatosCliente() {
            Console.WriteLine("\t Datos del cliente: ");
            Console.WriteLine($"\t - Nombre: {this.Cliente.Nombre}");
            Console.WriteLine($"\t - Dirección: {this.Cliente.Direccion}");
            Console.WriteLine($"\t - Teléfono: {this.Cliente.Telefono}\n");
        }

        public void VerDatosPedido() {

            Console.WriteLine($"\n\t NÚMERO DE PEDIDO: {this.Numero}");
            Console.WriteLine($"\t Observaciones: {this.Observaciones}");
            this.VerDatosCliente();
            Console.Write("\t Estado: ");

            switch(this.Estado) {

                case EstadoPedido.Pendiente:
                    Console.Write("Pendiente");
                break;

                case EstadoPedido.AsignadoACadete:
                    Console.Write("Asignado a un cadete");
                break;

                case EstadoPedido.Cancelado:
                    Console.Write("Cancelado");
                break;

                case EstadoPedido.EnCamino:
                    Console.Write("En camino");
                break;

                case EstadoPedido.EnPreparacion:
                    Console.Write("En preparación");
                break;

                case EstadoPedido.Entregado:
                    Console.Write("Entregado");
                break;

            }

            Console.Write("\n\n");
        }

        public string VerDireccionCliente() {
            return this.Cliente.Direccion;
        }

    }

}