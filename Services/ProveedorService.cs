using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PharmaStock.Models;

public class ProveedorService
{
    private readonly HttpClient _httpClient;

    public ProveedorService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task OrdenarReposicion(Producto producto)
    {
        var orden = new
        {
            ProductoId = producto.Id,
            Nombre = producto.Nombre,
            CantidadSolicitada = producto.CantidadMinima * 2, // Doble de la cantidad mínima
            FechaOrden = DateTime.UtcNow
        };

        string jsonOrden = JsonConvert.SerializeObject(orden);
        var content = new StringContent(jsonOrden, Encoding.UTF8, "application/json");

        // Suponiendo que el proveedor tiene una API en esta URL
        HttpResponseMessage response = await _httpClient.PostAsync("https://localhost:5274/api/proveedor/reponer", content);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Orden enviada para {producto.Nombre}");
        }
        else
        {
            Console.WriteLine($"Error al ordenar {producto.Nombre}");
        }
    }
}
