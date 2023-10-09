using EspacioPedido;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DatosPedidos;

public class AccesoADatosPedidos {

    public List<Pedido> Obtener() {

        if(File.Exists(Directory.GetCurrentDirectory() + "/pedidos.json")) {

            string json = File.ReadAllText(Directory.GetCurrentDirectory() + "/pedidos.json");

            List<Pedido> listadoPedidos = JsonSerializer.Deserialize<List<Pedido>>(json);

            Console.WriteLine("\n Datos de los pedidos leídos correctamente");

            return listadoPedidos;

        }
        else {
            Console.WriteLine("\n\n (!) No ha podido encontrarse el archivo de datos (pedidos.json)");
            return null;
        }

    }

    public void Guardar(List<Pedido> pedidos) {
        string json = JsonSerializer.Serialize(pedidos);
        string path = Directory.GetCurrentDirectory() + "/pedidos.json";
        File.WriteAllText(path, json);
        Console.WriteLine("\n Los pedidos se han guardado en el archivo \"pedidos.json\"");
    }

}