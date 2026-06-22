import { apiClient } from "../../shared/api/apiClient";

export type WeatherPredictionResponse = {
  location: string;
  temperature: number;
  rain: number;
  date: string;
};

export async function getWeatherPrediction(): Promise<WeatherPredictionResponse> {
  const response = await apiClient.get<WeatherPredictionResponse>(
    "/predictions/weather-test"
  );

  return response.data;
}