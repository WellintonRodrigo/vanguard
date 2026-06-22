import {
  CloudOutlined,
  DatabaseOutlined,
  HeartOutlined,
  SyncOutlined,
} from "@ant-design/icons";

import { StatusCard } from "../../shared/components/StatusCard";
import { useEffect, useState } from "react";
import {
  getCollectorSummary, getCollectorHistory,
} from "../health/collectorHealthService";
import type {
   CollectorHealthLogResponse,
   CollectorSummaryResponse,
  }from "../health/collectorHealthService";
import { 
  buildCommoditiesCard, 
  buildFreshnessCard, 
  buildHealthCard, 
  buildWeatherCard,

 } from "./dashboardCards";
import {
  getCommoditySummary, type CommoditySummaryResponse,
} from "../commodities/commodityService";
import {getWeatherPrediction, type WeatherPredictionResponse,
} from "../weather/weatherService";

export function DashboardPage() {

const [collectorSummary, setCollectorSummary] = useState<CollectorSummaryResponse[]>([]);
const [isLoading, setIsLoading] = useState(true);
const [collectorHistory, setCollectorHistory] = useState<CollectorHealthLogResponse[]>([]);
const [commoditySummary, setCommoditySummary] = 
useState<CommoditySummaryResponse | null>(null);
const [weatherPrediction, setWeatherPrediction] =
  useState<WeatherPredictionResponse | null>(null);

useEffect(() => {
  async function loadCollectorSummary() {
    try {
      const[summaryData, historyData, commodityData, WeatherData] = await Promise.all([
        getCollectorSummary(),
        getCollectorHistory(),
        getCommoditySummary(),
        getWeatherPrediction(),
      ])

      setCollectorSummary(summaryData);
      setCollectorHistory(historyData);
      setCommoditySummary(commodityData);
      setWeatherPrediction(WeatherData);

    } catch (error) {
      console.error("Erro ao buscar resumo dos coletores:", error);
    } finally {
      setIsLoading(false);
    }
  }

  loadCollectorSummary();
}, []);

const mainCollector = collectorSummary[0];
const latestHistory = collectorHistory[0];

const healthCard = buildHealthCard(mainCollector);
const freshnessCard = buildFreshnessCard(latestHistory);
const commoditiesCard = buildCommoditiesCard(commoditySummary);
const weatherCard = buildWeatherCard(weatherPrediction);


  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-slate-100">Dashboard</h1>
        <p className="mt-1 text-sm text-slate-400">
          Visão geral operacional do Projeto Vanguard.
        </p>
      </div>

      <div className="grid gap-4 md:grid-cols-2 xl:grid-cols-4">
        <StatusCard
          title="Saúde do Sistema"
          value={isLoading ? "Carregando..." : healthCard.value}
          description={healthCard.description}
          icon={<HeartOutlined />}
          status={healthCard.status}
        />
        

        <StatusCard
          title="Atualização dos Dados"
          value={isLoading ? "Carregando... ": freshnessCard.value}
          description={freshnessCard.description}
          icon={<SyncOutlined />}
          status={freshnessCard.status}
        />

        <StatusCard
          title="Clima"
          value={isLoading ? "Carregando..." : weatherCard.value}
          description={weatherCard.description}
          icon={<CloudOutlined />}
          status={weatherCard.status}
        />

        <StatusCard
          title="Commodities"
          value={isLoading ? "Carregando..." : commoditiesCard.value}
          description={commoditiesCard.description}
          icon={<DatabaseOutlined />}
          status={commoditiesCard.status}
        />
      </div>
    </div>
  );
}