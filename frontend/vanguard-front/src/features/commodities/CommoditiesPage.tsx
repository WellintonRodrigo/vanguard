import React, { useState, useEffect, useMemo } from 'react';
import { apiClient } from '../../shared/api/apiClient';
import { CommodityKPIs } from './CommodityKPIs';
import { formatCurrency, formatDate } from '../../shared/utils/formatters';
export interface CommodityPrice {
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
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedCommodity, setSelectedCommodity] = useState<string>('Todas');

  // 1. Requisita todas as cotações uma única vez ao montar a página
  useEffect(() => {
    const fetchAllCommodities = async () => {
      try {
        setLoading(true);
        setError(null);

        // O backend agora devolve tudo quando chamado sem parâmetros
        const response = await apiClient.get<CommodityPrice[]>('/commodity-prices/history');
        setData(response.data || []);
      } catch (err: unknown) {
        setError(err instanceof Error ? err.message : 'Erro ao carregar dados');
      } finally {
        setLoading(false);
      }
    };

    fetchAllCommodities();
  }, []);

  // 2. Extrai dinamicamente as commodities existentes no banco para o dropdown
  const availableCategories = useMemo(() => {
    if (!data || data.length === 0) return ['Todas'];
    const categories = Array.from(new Set(data.map((item) => item.commodity))).filter(Boolean);
    return ['Todas', ...categories.sort()];
  }, [data]);

  // 3. Filtro instantâneo em memória (0ms) ao trocar a opção no <select>
  const filteredData = useMemo(() => {
    if (selectedCommodity === 'Todas') {
      return data;
    }
    return data.filter(
      (item) => item.commodity.toLowerCase() === selectedCommodity.toLowerCase()
    );
  }, [data, selectedCommodity]);

  // 4. Recalcula os Cards de Destaque com base nos dados filtrados
  const latestCommodities = useMemo(() => {
    return getLatestPricesPerMarket(filteredData);
  }, [filteredData]);

  if (loading) {
    return (
      <div className="flex justify-center items-center min-h-[300px]">
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

        {/* Dropdown 100% Dinâmico */}
        <div className="flex items-center gap-2">
          <label htmlFor="commodity-select" className="text-xs font-medium text-gray-400">
            Commodity:
          </label>
          <select
            id="commodity-select"
            value={selectedCommodity}
            onChange={(e) => setSelectedCommodity(e.target.value)}
            className="px-3 py-1.5 text-xs font-semibold rounded-lg border border-gray-700 bg-gray-900 text-gray-200 focus:outline-none focus:ring-2 focus:ring-emerald-500 cursor-pointer transition-colors"
          >
            {availableCategories.map((item) => (
              <option key={item} value={item}>
                {item}
              </option>
            ))}
          </select>
        </div>
      </div>

      <CommodityKPIs data={filteredData} />

      {/* Cards de Destaque */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {latestCommodities.map((item) => {
          const isPositive = (item.dailyVariationPercent ?? 0) >= 0;
          return (
            <div
              key={item.id}
              className="p-5 bg-white dark:bg-gray-900 border border-gray-200 dark:border-gray-800 rounded-xl shadow-sm hover:shadow-md transition-shadow"
            >
              <div className="flex justify-between items-start mb-2">
                <div>
                  <span className="text-xs font-semibold tracking-wider text-emerald-500 uppercase">
                    {item.commodity}
                  </span>
                  <h3 className="text-base font-semibold text-gray-800 dark:text-gray-100">
                    {item.market}
                  </h3>
                </div>
                <span
                  className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-semibold ${
                    isPositive
                      ? 'bg-emerald-100 text-emerald-800 dark:bg-emerald-950 dark:text-emerald-300'
                      : 'bg-rose-100 text-rose-800 dark:bg-rose-950 dark:text-rose-300'
                  }`}
                >
                  {isPositive ? '▲' : '▼'}
                  {item.dailyVariationPercent}%
                </span>
              </div>
              <div className="mt-4 flex items-baseline justify-between">
                <div>
                  <span className="text-3xl font-extrabold text-gray-900 dark:text-white">
                    {formatCurrency(item.priceBrl)}
                  </span>
                  <span className="ml-1 text-xs text-gray-500">/ {item.unit}</span>
                </div>
              </div>
              <div className="mt-4 pt-3 border-t border-gray-100 dark:border-gray-800 flex justify-between text-xs text-gray-400">
                <span>Fonte: {item.source}</span>
                <span>Ref: {formatDate(item.referenceDate)}</span>
              </div>
            </div>
          );
        })}
      </div>

      {/* Tabela de Histórico */}
      <div className="bg-white dark:bg-gray-900 border border-gray-200 dark:border-gray-800 rounded-xl overflow-hidden shadow-sm">
        <div className="p-4 border-b border-gray-200 dark:border-gray-800">
          <h2 className="text-lg font-semibold text-gray-800 dark:text-gray-200">
            Histórico de Coletas
          </h2>
        </div>
        <div className="overflow-x-auto">
          <table className="w-full text-left text-sm text-gray-500 dark:text-gray-300">
            <thead className="bg-gray-50 dark:bg-gray-800 text-xs text-gray-500 uppercase tracking-wider">
              <tr>
                <th className="px-6 py-3">Data Ref.</th>
                <th className="px-6 py-3">Commodity</th>
                <th className="px-6 py-3">Mercado</th>
                <th className="px-6 py-3">Preço (BRL)</th>
                <th className="px-6 py-3">Variação Diária</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-200 dark:divide-gray-800">
              {filteredData.length > 0 ? (
                filteredData.map((row) => (
                  <tr
                    key={row.id}
                    className="hover:bg-gray-50 dark:hover:bg-gray-800/50 transition-colors"
                  >
                    <td className="px-6 py-4 font-medium whitespace-nowrap">
                      {formatDate(row.referenceDate)}
                    </td>
                    <td className="px-6 py-4">{row.commodity}</td>
                    <td className="px-6 py-4">{row.market}</td>
                    <td className="px-6 py-4 font-semibold text-gray-900 dark:text-white">
                      {formatCurrency(row.priceBrl)}
                    </td>
                    <td
                      className={`px-6 py-4 font-medium ${
                        (row.dailyVariationPercent ?? 0) >= 0 ? 'text-emerald-500' : 'text-rose-500'
                      }`}
                    >
                      {row.dailyVariationPercent}%
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan={5} className="px-6 py-8 text-center text-gray-500">
                    Nenhuma cotação encontrada.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};