import { apiClient } from "../../shared/api/apiClient";

export type CollectorSummaryResponse = {
  source: string;
  totalChecks: number;
  healthyChecks: number;
  warningChecks: number;
  criticalChecks: number;
  uptimePercent: number;
  averageResponseTimeMs: number;
};

export type CollectorHealthLogResponse = {
  id: string;
  source: string;
  commodity: string;
  status: "Healthy" | "Warning" | "Critical";
  httpStatusCode: number;
  recordsFound: number;
  responseTimeMs: number;
  errorMessage: string | null;
  checkedAt: string;
};

export async function getCollectorSummary(): Promise<CollectorSummaryResponse[]> {
  const response = await apiClient.get<CollectorSummaryResponse[]>(
    "/collector/summary"
  );

  return response.data;
}

export async function getCollectorHistory(): Promise<CollectorHealthLogResponse[]> {
  const response = await apiClient.get<CollectorHealthLogResponse[]>(
    "/collector/history"
  );

  return response.data;
}