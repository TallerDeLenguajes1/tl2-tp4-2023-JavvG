using Web_Api;
using EspacioCadete;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace DatosCadetes;

public class AccesoADatosCadetes {

    public List<Cadete> Obtener() {
        if(File.Exists(Directory.GetCurrentDirectory() + "/cadetes.json")) {

            string json = File.ReadAllText(Directory.GetCurrentDirectory() + "/cadetes.json");

            List<Cadete> listaCadetes = JsonSerializer.Deserialize<List<Cadete>>(json);

            Console.WriteLine("\n Datos de los cadetes leídos correctamente");

            return listaCadetes;
        }
        else {
            Console.WriteLine("\n\n (!) No ha podido encontrarse el archivo de datos (cadetes.json)");
            return null;
        }
    }

    public void Guardar(List<Cadete> cadetes) {
        string json = JsonSerializer.Serialize(cadetes);
        string path = Directory.GetCurrentDirectory() + "/cadetes.json";
        File.WriteAllText(path, json);
        Console.WriteLine("\n Los cadetes se han guardado en el archivo \"cadetes.json\"");
    }


}