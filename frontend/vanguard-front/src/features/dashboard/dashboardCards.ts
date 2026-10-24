import type {
  CollectorHealthLogResponse,
  CollectorSummaryResponse,
} from "../health/collectorHealthService";
import { formatDateTime } from "../../shared/utils/dateUtils";
import type { CommoditySummaryResponse }from "../commodities/commodityService";
import type { WeatherResponse } from "../weather/weatherService";

type CardStatus = "success" | "warning" | "error" | "info";

export function buildHealthCard(summary?: CollectorSummaryResponse) {
  return {
    value: summary ? `${summary.uptimePercent}%` : "Offline",
    description: summary
      ? `${summary.source} • ${summary.averageResponseTimeMs}ms`
      : "Resumo dos coletores indisponível",
    status: !summary
      ? "error"
      : summary.criticalChecks > 0
        ? "error"
        : summary.warningChecks > 0
          ? "warning"
          : "success" as CardStatus,
  };
}

export function buildFreshnessCard(history?: CollectorHealthLogResponse) {
  return {
    value: history ? "Atualizado" : "Indisponível",
    description: history
      ? `Última coleta: ${formatDateTime(history.checkedAt)}`
      : "Histórico dos coletores indisponível",
    status: !history
      ? "error"
      : history.status === "Healthy"
        ? "success"
        : history.status === "Warning"
          ? "warning"
          : "error" as CardStatus,
  };
}

export function buildCommoditiesCard(
  summary?: CommoditySummaryResponse | null | undefined
) {
  return {
    value: summary
      ? `${summary.totalCommodities} monitoradas`
      : "Indisponível",

    description: summary
      ? summary.commodities.join(" • ")
      : "Sem dados de commodities",

    status: summary
      ? "success"
      : "error" as CardStatus,
  };
}

export function buildWeatherCard(
  weather?: WeatherResponse | null
) {
  const temp = weather?.temperatureMax ?? weather?.temperature ?? 0;
  // Pega a chuva em mm
  const rain = weather?.precipitationMm ?? weather?.rain ?? 0;
  const location = weather?.location ?? "Joinville - SC";
  return {
    value: weather ? `${temp}°C` : "Indisponível",
    description: weather
      ? `${location} • Chuva ${rain}mm`
      : "Previsão climática indisponível",
    status: weather ? "info" : "error" as CardStatus,
  };
}