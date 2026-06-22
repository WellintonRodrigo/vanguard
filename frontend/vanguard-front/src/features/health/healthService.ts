import { apiClient } from "../../shared/api/apiClient";

export type HealthResponse = {
  status: string;
  message?: string;
  checkedAt?: string;
};

export async function getHealth(): Promise<HealthResponse> {
  const response = await apiClient.get<HealthResponse>("/health");

  return response.data;
}