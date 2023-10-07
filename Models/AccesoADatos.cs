using System.ComponentModel;
using System.IO.Enumeration;
using EspacioCadete;
using EspacioCadeteria;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace EspacioAccesoADatos;

public abstract class AccesoADatos{

    public virtual Cadeteria LeerDatosCadeteria(string filename){
        return null;
    }

    public virtual List<Cadete> LeerDatosCadetes(string filename) {
        return null;
    }

}

public class AccesoCSV : AccesoADatos {     // La subclase hereda los atributos y métodos de la clase base AccesoADatos

    public override Cadeteria LeerDatosCadeteria(string filename) {

        if(File.Exists(filename)) {     // Si el archivo existe, ejecutar lo siguiente

            List<Cadeteria> listaCadeteria = new();

            using (var reader = new StreamReader(filename)) {

                while(!reader.EndOfStream) {        // Mientras no acabe la lectura del archivo

                    string line = reader.ReadLine();       // Se lee una línea de archivo

                    if(line != null) {      // Si la línea leída no está vacía, ejecutar lo siguiente

                        var splits = line.Split(',');       // Separa las línea leída en el caracter ','

                        Cadeteria cadeteria = new();

                        cadeteria.Nombre = splits[0].Trim();        // El primer split corresponde al nombre (Trim() remueve los espacios en blanco)
                        cadeteria.Telefono = int.Parse(splits[1].Trim());       // El segundo split corresponde al teléfono, haciendo la conversión a entero

                        listaCadeteria.Add(cadeteria);

                    }
                }
            }

            Console.WriteLine("\n Datos de la cadeteria leídos correctamente");
            return listaCadeteria[0];
        }
        else {
            Console.WriteLine("\n\n (!) No ha podido encontrarse el archivo de datos (cadeteria.csv)");
            return null;
        }
    }

    public override List<Cadete> LeerDatosCadetes(string filename) {

        if(File.Exists(filename)) {

            List<Cadete> listaCadetes = new();

            using (var reader = new StreamReader(filename)) {

                while(!reader.EndOfStream) {

                    string line = reader.ReadLine();

                    if(line != null) {

                        var splits = line.Split(',');

                        Cadete cadete = new Cadete();       // Nueva instancia para crear un cadete

                        cadete.IdCadete = int.Parse(splits[0].Trim());
                        cadete.Nombre = splits[1].Trim();
                        cadete.Direccion = splits[2].Trim();
                        cadete.Telefono = long.Parse(splits[3].Trim());

                       listaCadetes.Add(cadete);        // Se añade el nuevo cadete registrado a la lista

                    }

                }

            }

            Console.WriteLine("\n Datos de los cadetes leídos correctamente");

            return listaCadetes;

        }
        else {
            Console.WriteLine("\n\n (!) No ha podido encontrarse el archivo de datos (cadetes.csv)");
            return null;
        }

    }

}

public class AccesoJSON : AccesoADatos {

    public override Cadeteria LeerDatosCadeteria(string filename) {
        
        if(File.Exists(filename)) {

            List<Cadeteria> listaCadeteria = new();

            string JSON = File.ReadAllText(filename);

            listaCadeteria = JsonSerializer.Deserialize<List<Cadeteria>>(JSON);       // Se añaden los datos leídos del archivo JSON a un arreglo

            Console.WriteLine("\n Datos de la cadetería leídos correctamente");

            return listaCadeteria[0];

        }
        else {
            Console.WriteLine("\n\n (!) No ha podido encontrarse el archivo de datos (cadeteria.json)");
            return null;
        }
        
    }

    public override List<Cadete> LeerDatosCadetes(string filename) {

        if(File.Exists(filename)) {

            List<Cadete> listaCadetes = new();

            string JSON = File.ReadAllText(filename);

            listaCadetes = JsonSerializer.Deserialize<List<Cadete>>(JSON);

            Console.WriteLine("\n Datos de los cadetes leídos correctamente");

            return listaCadetes;
        }
        else {
            Console.WriteLine("\n\n (!) No ha podido encontrarse el archivo de datos (cadetes.json)");
            return null;
        }

    }


}
