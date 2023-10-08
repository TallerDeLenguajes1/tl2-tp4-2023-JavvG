using Web_Api;
using EspacioCadete;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace DatosCadetes;

public class AccesoADatosCadetes {

    public List<Cadete> Obtener() {
        if(File.Exists("cadetes.json")) {

            string json = File.ReadAllText("cadetes.json");

            List<Cadete> listaCadetes = JsonSerializer.Deserialize<List<Cadete>>(json);

            Console.WriteLine("\n Datos de los cadetes leídos correctamente");

            return listaCadetes;
        }
        else {
            Console.WriteLine("\n\n (!) No ha podido encontrarse el archivo de datos (cadetes.json)");
            return null;
        }
    }

}