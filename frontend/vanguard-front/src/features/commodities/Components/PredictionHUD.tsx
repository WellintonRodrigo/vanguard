import React, { useState } from 'react';
import {
  ThunderboltOutlined,
  DownOutlined,
  RobotOutlined,
  AimOutlined,
  LineChartOutlined,
  AlertOutlined,
} from '@ant-design/icons';
import { apiClient } from '../../../shared/api/apiClient';
import { formatTrend } from '../../../shared/utils/formatTrend';

export interface AgriculturePredictionRequest {
  product: string;
  location: string;
  latitude: number;
  longitude: number;
}

export interface AgriculturePredictionResponse {
  product: string;
  location: string;
  impactProbability: number;
  trend: string;
  estimatedVariationPercent: number;
  insight: string;
  predictionDate: string;
}

export interface DailyForecastDto {
  date: string;
  precipitationSum: number;
  temperatureMax: number;
  temperatureMin: number;
}

export interface AgricultureForecastResponse {
  product: string;
  location: string;
  impactProbability: number;
  trend: string;
  estimatedVariationPercent: number;
  insight: string;
  predictionDate: string;
  totalRainAccumulated: number;
  criticalDaysCount: number;
  dailyForecasts: DailyForecastDto[];
}

export const PredictionHUD: React.FC = () => {
  const [isExpanded, setIsExpanded] = useState<boolean>(true);
  const [product, setProduct] = useState<string>('Soja');
  const [location, setLocation] = useState<string>('Joinville - SC');
  const [latitude] = useState<number>(-26.3044);
  const [longitude] = useState<number>(-48.8487);

  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const [forecastData, setForecastData] = useState<AgricultureForecastResponse | null>(null);

  const handleRunPrediction = async () => {
    setLoading(true);
    setError(null);

    try {
      const payload: AgriculturePredictionRequest = {
        product,
        location,
        latitude,
        longitude,
      };

      const response = await apiClient.post<AgricultureForecastResponse>(
        '/predictions/agriculture/forecast',
        payload
      );

      setForecastData(response.data);
    } catch (err: unknown) {
      console.error('Erro ao executar forecast preditivo:', err);
      setError(
        'Falha ao conectar com o motor Vanguard Engine. Verifique se a API está online.'
      );
    } finally {
      setLoading(false);
    }
  };

  const trendInfo = forecastData
    ? formatTrend(forecastData.trend)
    : { label: '', className: '' };

  return (
    <div className="bg-slate-900 border border-slate-800 rounded-xl overflow-hidden shadow-2xl transition-all duration-300 mb-6">
      {/* Cabeçalho do HUD Preditivo */}
      <button
        type="button"
        onClick={() => setIsExpanded(!isExpanded)}
        className="w-full p-4 bg-slate-900 hover:bg-slate-800/70 transition-colors flex items-center justify-between border-b border-slate-800/80 focus:outline-none"
      >
        <div className="flex items-center gap-3">
          <div className="p-2.5 rounded-lg bg-emerald-950/60 border border-emerald-800/40 text-emerald-400 text-lg flex items-center justify-center">
            <RobotOutlined />
          </div>
          <div className="text-left">
            <div className="flex items-center gap-2">
              <h2 className="text-base font-semibold text-slate-100">
                HUD Preditivo Vanguard
              </h2>
              <span className="text-[10px] font-bold px-2 py-0.5 rounded bg-emerald-950 text-emerald-400 border border-emerald-800/50">
                v2.0 ENGINE
              </span>
            </div>
            <p className="text-xs text-slate-400">
              Análise em tempo real de risco climático e volatilidade de mercado
            </p>
          </div>
        </div>

        <div className="flex items-center gap-3">
          <span className="text-xs text-slate-400 font-medium hidden sm:inline">
            {isExpanded ? 'Recolher HUD' : 'Expandir HUD'}
          </span>
          <div
            className={`p-1.5 rounded-lg bg-slate-800 text-slate-300 transform transition-transform duration-300 text-xs flex items-center justify-center ${isExpanded ? 'rotate-180 bg-emerald-950 text-emerald-400' : ''
              }`}
          >
            <DownOutlined />
          </div>
        </div>
      </button>

      {/* Conteúdo Expansível do HUD */}
      <div
        className={`transition-all duration-300 ease-in-out overflow-hidden ${isExpanded ? 'max-h-250 opacity-100 p-5' : 'max-h-0 opacity-0 p-0'
          }`}
      >
        {/* Controles de Entrada / Parâmetros */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4 bg-slate-950/60 border border-slate-800/80 p-4 rounded-xl">
          <div>
            <label className="block text-xs font-semibold text-slate-400 mb-1 items-center gap-1.5">
              <AimOutlined className="text-emerald-400" /> COMMODITY TARGET
            </label>
            <select
              value={product}
              onChange={(e) => setProduct(e.target.value)}
              className="w-full bg-slate-900 border border-slate-700/80 rounded-lg px-3 py-2 text-sm text-slate-100 focus:outline-none focus:border-emerald-500 transition-colors"
            >
              <option value="Soja">Soja</option>
              <option value="Milho">Milho</option>
              <option value="Boi-Gordo">Boi-Gordo</option>
              <option value="Arroz">Arroz</option>
            </select>
          </div>

          <div>
            <label className="block text-xs font-semibold text-slate-400 mb-1 items-center gap-1.5">
              <LineChartOutlined className="text-emerald-400" /> REGIONAL DE ANÁLISE
            </label>
            <input
              type="text"
              value={location}
              onChange={(e) => setLocation(e.target.value)}
              className="w-full bg-slate-900 border border-slate-700/80 rounded-lg px-3 py-2 text-sm text-slate-100 focus:outline-none focus:border-emerald-500 transition-colors"
            />
          </div>

          <div className="flex items-end">
            <button
              onClick={handleRunPrediction}
              disabled={loading}
              className="w-full bg-emerald-600 hover:bg-emerald-500 disabled:bg-slate-800 text-white font-bold py-2 px-4 rounded-lg transition-all duration-200 flex items-center justify-center gap-2 shadow-lg shadow-emerald-950/50"
            >
              {loading ? (
                <>
                  <span className="animate-spin h-4 w-4 border-2 border-white border-t-transparent rounded-full"></span>
                  <span className="text-xs">PROCESSANDO HEURÍSTICA...</span>
                </>
              ) : (
                <>
                  <ThunderboltOutlined />
                  <span className="text-xs">EXECUTAR ANÁLISE PREDITIVA</span>
                </>
              )}
            </button>
          </div>
        </div>

        {/* Mensagem de Erro */}
        {error && (
          <div className="mt-4 p-4 bg-rose-950/40 border border-rose-800/60 rounded-xl text-rose-300 text-xs flex items-center gap-2">
            <AlertOutlined />
            <span>{error}</span>
          </div>
        )}

        {/* Painel de Resultados (Sci-Fi Cards) */}
        {forecastData && (
          <div className="mt-5 p-5 bg-slate-950/80 border border-slate-800/90 rounded-xl space-y-5">
            <div className="grid grid-cols-1 md:grid-cols-3 gap-4 border-b border-slate-800/80 pb-5 text-center">
              {/* Risco / Impacto */}
              <div className="bg-slate-900/60 border border-slate-800/60 p-3.5 rounded-lg">
                <span className="text-[11px] font-medium text-slate-400 uppercase tracking-wider block mb-1">
                  PROBABILIDADE DE IMPACTO
                </span>
                <span className="text-3xl font-extrabold text-emerald-400">
                  {forecastData.impactProbability}%
                </span>
              </div>

              {/* Tendência */}
              <div className="bg-slate-900/60 border border-slate-800/60 p-3.5 rounded-lg">
                <span className="text-[11px] font-medium text-slate-400 uppercase tracking-wider block mb-1">
                  TENDÊNCIA DE MERCADO
                </span>
                <span className={`text-2xl font-bold uppercase ${trendInfo.className}`}>
                  {trendInfo.label}
                </span>
              </div>

              {/* Variação Estimada */}
              <div className="bg-slate-900/60 border border-slate-800/60 p-3.5 rounded-lg">
                <span className="text-[11px] font-medium text-slate-400 uppercase tracking-wider block mb-1">
                  VARIAÇÃO DE PREÇO PREVISTA
                </span>
                <span className="text-2xl font-bold text-sky-400">
                  ~{forecastData.estimatedVariationPercent}%
                </span>
              </div>
            </div>

            {/* Horizonte Meteorológico de 7 Dias */}
            {forecastData?.dailyForecasts && forecastData.dailyForecasts.length > 0 && (
              <div className="bg-slate-950/80 border border-slate-800/90 p-4 rounded-xl space-y-3">
                <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center border-b border-slate-800/80 pb-2.5 gap-2">
                  <span className="text-xs text-emerald-400 font-bold uppercase tracking-wider">
                    📅 HORIZONTE METEOROLÓGICO (PRÓXIMOS 7 DIAS)
                  </span>
                  <div className="flex items-center gap-4 text-[11px] text-slate-400 font-mono">
                    <span>
                      Chuva Acumulada: <strong className="text-white">{forecastData.totalRainAccumulated ?? 0} mm</strong>
                    </span>
                    <span>
                      Dias Críticos:{' '}
                      <strong className={(forecastData.criticalDaysCount ?? 0) > 0 ? 'text-amber-400' : 'text-emerald-400'}>
                        {forecastData.criticalDaysCount ?? 0}
                      </strong>
                    </span>
                  </div>
                </div>

                {/* Cards diários */}
                <div className="grid grid-cols-2 sm:grid-cols-4 lg:grid-cols-7 gap-2.5 pt-1">
                  {forecastData?.dailyForecasts.map((day, index) => {
                    const isCritical = day.precipitationSum >= 20;
                    return (
                      <div
                        key={index}
                        className={`p-3 rounded-lg border text-center transition-all ${isCritical
                            ? 'bg-amber-950/35 border-amber-800/60 text-amber-200'
                            : 'bg-slate-900/60 border-slate-800/60 text-slate-300'
                          }`}
                      >
                        <div className="text-[10px] text-slate-400 font-mono mb-1">
                          {new Date(day.date).toLocaleDateString('pt-BR', {
                            weekday: 'short',
                            day: '2-digit',
                            month: '2-digit',
                          })}
                        </div>
                        <div className="text-sm font-bold flex items-center justify-center gap-1 my-1">
                          <span className={isCritical ? 'text-amber-400' : 'text-sky-400'}>💧</span>
                          <span>{day.precipitationSum} mm</span>
                        </div>
                        <div className="text-[10px] text-slate-400">
                          {day.temperatureMin}° / {day.temperatureMax}°
                        </div>
                      </div>
                    );
                  })}
                </div>
              </div>
            )}

            {/* Diagnóstico / Insight em destaque */}
            <div className="space-y-2">
              <span className="text-[11px] text-emerald-400 font-bold uppercase tracking-wider flex items-center gap-1.5">
                &gt; DIAGNÓSTICO HEURÍSTICO VANGUARD
              </span>
              <div className="p-4 bg-slate-900/90 border border-emerald-950/80 rounded-lg text-slate-200 text-sm font-mono leading-relaxed shadow-inner">
                "{forecastData.insight}"
              </div>
            </div>

            <div className="text-right text-[10px] text-slate-500 font-mono">
              Análise gerada em:{' '}
              {new Date(forecastData.predictionDate).toLocaleString('pt-BR')}
            </div>
          </div>
        )}
      </div>
    </div>
  );
};