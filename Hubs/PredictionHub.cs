using System;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using PharmaStock.Models;
using Python.Runtime;

namespace PharmaStock.Hubs
{
    public class PredictionHub : Hub
    {
        public async Task SendPrediction(string csvFilePath)
        {
            try
            {
                // Ejecutar el script de Python y generar datos CSV
                RunPythonScript();

                // Convertir el archivo CSV en JSON
                string jsonData = CsvToJson.ConvertCsvToJson(csvFilePath);

                // Enviar los datos JSON a todos los clientes conectados
                await Clients.All.SendAsync("ReceivePrediction", jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SendPrediction: {ex.Message}");
                throw;
            }
        }

        private void RunPythonScript()
        {
            try
            {
                using (Py.GIL())
                {
                    // Importar y ejecutar el módulo de Python
                    dynamic py = Py.Import("Demand_Prediction");
                    py.predictionData(); // Llamar a la función del script de Python
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al ejecutar el script de Python: {ex.Message}");
                throw;
            }
        }
    }
}
