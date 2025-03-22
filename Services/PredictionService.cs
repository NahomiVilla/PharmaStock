using System;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using PharmaStock.Hubs;
using PharmaStock.Models;
using Python.Runtime;

namespace PharmaStock.Services{
    public class PredictionService{
        private readonly IHubContext<PredictionHub> _hubContext;
        

        public PredictionService(IHubContext<PredictionHub> hubContext){
            _hubContext = hubContext;
           
        }

        public async Task FetchAndBroadcastPrediction()
        {
            string result = RunPythonScript();

            if (!string.IsNullOrEmpty(result))
            {
                await _hubContext.Clients.All.SendAsync("ReceivePrediction", result);
            }
        }

        private string RunPythonScript()
        {
            try
            {
                // Ruta al script de predicción en Python
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string pythonScript = Path.Combine(basePath, "PythonScripts/Demand_Prediction.py");
                Console.WriteLine($"Ejecutando script Python: {pythonScript}");
                string pythonExe = "python3"; // O usa la ruta completa si es necesario

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = pythonExe,
                    Arguments = $"\"{pythonScript}\"",  // Ejecuta el script
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process { StartInfo = psi })
                {
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine($"Error en script Python: {error}");
                        return string.Empty;
                    }

                    return output;  // Devuelve la predicción en formato JSON
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ejecutando Python: {ex.Message}");
                return string.Empty;
            }
        }

    }

    

}
