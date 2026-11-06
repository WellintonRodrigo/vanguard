import React, { useState, useEffect, useMemo } from 'react';
import { apiClient } from '../../../shared/api/apiClient';
import { CommodityKPIs } from './CommodityKPIs';
import { CommodityHistoryTable } from './CommodityHistoryTable';
import { CommoditesCardsDest } from './CommoditesCardsDest';
import { PredictionHUD } from './PredictionHUD';
import { formatCurrency, formatDate } from '../../../shared/utils/formatters';
import { CommodityFilter } from './CommodityFilter';
export interface CommodityPrice {
  [key: string]: string | number | null;
  id: string;
  source: string;
  commodity: string;
  market: string;
  unit: string;
  priceBrl: number;
  priceUsd: number | null;
  dailyVariationPercent: number | null;
  monthlyVariationPercent: number | null;
  referenceDate: string;
  collectedAt: string;
}

function getLatestPricesPerMarket(items: CommodityPrice[]): CommodityPrice[] {
  const map = new Map<string, CommodityPrice>();

  items.forEach((item) => {
    const key = `${item.commodity}-${item.market}`;
    const existing = map.get(key);

    if (!existing || new Date(item.referenceDate) > new Date(existing.referenceDate)) {
      map.set(key, item);
    }
  });

  return Array.from(map.values());
}

export const CommoditiesPage: React.FC = () => {
  const [data, setData] = useState<CommodityPrice[]>([]);
  const [filteredData, setFilteredData] = useState<CommodityPrice[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [activeTab, setActiveTab] = useState<'central' | 'audit'>('central');

  const handleFilteredDataChange: NonNullable<
    React.ComponentProps<typeof CommodityFilter>['onFilteredDataChange']
  > = (filtered) => {
    setFilteredData(filtered as CommodityPrice[]);
  };

  // 1. Requisita todas as cotações uma única vez ao montar a página
  useEffect(() => {
    const fetchAllCommodities = async () => {
      try {
        setLoading(true);
        setError(null);

        // O backend agora devolve tudo quando chamado sem parâmetros
        const response = await apiClient.get<CommodityPrice[]>('/commodity-prices/history');
        const prices = response.data || [];
        setData(prices);
        setFilteredData(prices);
      } catch (err: unknown) {
        setError(err instanceof Error ? err.message : 'Erro ao carregar dados');
      } finally {
        setLoading(false);
      }
    };

    fetchAllCommodities();
  }, []);

  
  // 4. Recalcula os Cards de Destaque com base nos dados filtrados
  const latestCommodities = useMemo(() => {
    return getLatestPricesPerMarket(data);
  }, [data]);

  if (loading) {
    return (
      <div className="flex justify-center items-center min-h-75">
        <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-emerald-500"></div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="p-4 bg-rose-50 dark:bg-rose-950/50 border border-rose-200 dark:border-rose-800 rounded-lg text-rose-700 dark:text-rose-300">
        <p className="font-semibold">Falha ao carregar cotações de commodities.</p>
        <p className="text-sm mt-1">{error}</p>
      </div>
    );
  }

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-8">
      {/* Header e Filtro Dinâmico */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-gray-200 dark:border-gray-800 pb-4">
        <div>
          <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Cotações de Commodities</h1>
          <p className="text-sm text-gray-500 dark:text-gray-400">Acompanhamento em tempo real via Vanguard API</p>
        </div>

        {/* ABAS MODULARES */}
        <div className="flex items-center bg-slate-950 border border-slate-800 p-1 rounded-xl shadow-lg">
          <button
            onClick={() => setActiveTab('central')}
            className={`px-4 py-2 rounded-lg text-xs font-semibold transition-all duration-200 ${activeTab === 'central'
                ? 'bg-emerald-600 text-white shadow-md'
                : 'text-slate-400 hover:text-white hover:bg-slate-900'
              }`}
          >
            Central Preditiva &amp; Cotações
          </button>
          <button
            onClick={() => setActiveTab('audit')}
            className={`px-4 py-2 rounded-lg text-xs font-semibold transition-all duration-200 ${activeTab === 'audit'
                ? 'bg-emerald-600 text-white shadow-md'
                : 'text-slate-400 hover:text-white hover:bg-slate-900'
              }`}
          >
            Histórico de Auditoria
          </button>
        </div>

      </div>

      {activeTab === 'central' ? (
        <>

          <CommodityKPIs data={filteredData} />

          <div className="p-6 space-y-6">
            <PredictionHUD />
          </div>

          <CommoditesCardsDest
            data={latestCommodities}
            formatCurrency={formatCurrency}
            formatDate={formatDate}
          />

        </>
      ) : (
        <div>
          <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-gray-200 dark:border-gray-800 pb-4">
            <CommodityFilter
              data={data}
              onFilteredDataChange={handleFilteredDataChange}
            />
          </div>

          <div className="p-6 space-y-6">
            <CommodityHistoryTable
              data={filteredData}
              formatCurrency={formatCurrency}
              formatDate={formatDate}
            />
          </div>
        </div>
      )}
    </div>
  );
};