import { apiClient } from "../../shared/api/apiClient";

// Contrato leve de Clima Atual
export interface WeatherLog {
  location: string;
  temperatureMin?: number;
  temperatureMax?: number;
  precipitationMm?: number;
  temperature: number; // ou decimal retornado pelo OpenMeteo
  rain: number;        // precipitação em mm
  date?: string;
}

// Busca / Sincroniza o clima atual da região para o Card do Dashboard
export async function getWeatherPrediction(): Promise<WeatherLog | null> {
  
  const response = await apiClient.get<WeatherLog[]>('/api/weather/history', {
    params: {location: 'Joinville - SC',},
  });
  // Retorna o último registro salvo (o mais recente)
  if(Array.isArray(response.data) && response.data.length > 0) {
    return response.data[0];
  }

  return null;
}