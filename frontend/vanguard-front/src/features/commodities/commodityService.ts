import { apiClient } from './../../shared/api/apiClient';

export type CommoditySummaryResponse = {
  totalRecords: number;
  totalCommodities: number;
  commodities: string[];
  totalSources: number;
  sources: string[];
  lastCollection: string;
};

export async function getCommoditySummary(): Promise<CommoditySummaryResponse> {
  const response = await apiClient.get<CommoditySummaryResponse>(
    "/commodity-prices/summary"
  );
  return response.data
}