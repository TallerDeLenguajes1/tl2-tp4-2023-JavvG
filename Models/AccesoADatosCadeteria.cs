using Web_Api;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace DatosCadeteria;

public class AccesoADatosCadeteria {

    public Cadeteria Obtener() {
        if(File.Exists(Directory.GetCurrentDirectory() + "/cadeteria.json")) {

            string json = File.ReadAllText(Directory.GetCurrentDirectory() + "/cadeteria.json");

            List<Cadeteria> lista = JsonSerializer.Deserialize<List<Cadeteria>>(json);

            Console.WriteLine("\n Datos de los cadeteria leídos correctamente");

            return lista[0];
        }
        else {
            Console.WriteLine("\n\n (!) No ha podido encontrarse el archivo de datos (cadeteria.json)");
            return null;
        }
    }


}