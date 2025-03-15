using System;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using PharmaStock.Hubs;
using PharmaStock.Models;
using Python.Runtime;

namespace PharmaStock.Services{
    class PredictionService{
        private readonly IHubContext<PredictionHub> _hubContext;
        private readonly HttpClient _httpClient;

        public PredictionService(IHubContext<PredictionHub> hubContext, HttpClient httpClient){
            _hubContext = hubContext;
            _httpClient = httpClient;
        }

        public async Task FetchAndBroadcastPrediction(){
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5000/predicciones");
            if(response.IsSuccessStatusCode){
                string json = await response.Content.ReadAsStringAsync();
                await _hubContext.Clients.All.SendAsync("ReceivePrediction", json);
            }
        }
    }

    

}
