namespace EspacioCliente {

    public class Cliente {
        private string nombre;
        private string direccion;
        private long telefono;

        public string Nombre { get => nombre; set => nombre = value; }
        public string Direccion { get => direccion; set => direccion = value; }
        public long Telefono { get => telefono; set => telefono = value; }

        public Cliente(string nombre, string direccion, long telefono) {
            this.Nombre = nombre;
            this.Direccion = direccion;
            this.Telefono = telefono;
        }

    }

}