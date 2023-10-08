using Web_Api;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace DatosCadeteria;

public class AccesoADatosCadeteria {

    public Cadeteria Obtener() {

        if(File.Exists("cadeteria.json")) {

            string json = File.ReadAllText("cadeteria.json");

            Cadeteria cadeteria = JsonSerializer.Deserialize<Cadeteria>(json);       // Se añaden los datos leídos del archivo JSON a un arreglo

            Console.WriteLine("\n Datos de la cadetería leídos correctamente");

            return cadeteria;

        }
        else {
            Console.WriteLine("\n\n (!) No ha podido encontrarse el archivo de datos (cadeteria.json)");
            return null;
        }
        
    }

}