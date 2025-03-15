using System.Data;
using System.IO;
using System.Text.Json;

public class CsvToJson
{
    public static string ConvertCsvToJson(string filePath)
    {
        var lines = File.ReadAllLines(filePath);

        // Verificar si el archivo CSV tiene datos
        if (lines.Length < 2)
        {
            throw new Exception("El archivo CSV no contiene datos suficientes.");
        }

        // Leer encabezados del CSV
        var headers = lines[0].Split(',');

        // Crear lista de diccionarios (cada fila será un diccionario con encabezados como claves)
        var data = new List<Dictionary<string, string>>();
        for (int i = 1; i < lines.Length; i++)
        {
            var row = lines[i].Split(',');
            var rowDict = new Dictionary<string, string>();
            for (int j = 0; j < headers.Length; j++)
            {
                rowDict[headers[j]] = row[j];
            }
            data.Add(rowDict);
        }

        // Convertir lista de diccionarios a JSON
        return JsonSerializer.Serialize(data);
    }
}
