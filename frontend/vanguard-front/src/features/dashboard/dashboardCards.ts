import type {
  CollectorHealthLogResponse,
  CollectorSummaryResponse,
} from "../health/collectorHealthService";
import { formatDateTime } from "../../shared/utils/dateUtils";
import type { CommoditySummaryResponse }from "../commodities/commodityService";
import type { WeatherPredictionResponse } from "../weather/weatherService";

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
  weather?: WeatherPredictionResponse | null
) {
  return {
    value: weather ? `${weather.temperature}°C` : "Indisponível",
    description: weather
      ? `${weather.location} • Chuva ${weather.rain}mm`
      : "Previsão climática indisponível",
    status: weather ? "info" : "error" as CardStatus,
  };
}