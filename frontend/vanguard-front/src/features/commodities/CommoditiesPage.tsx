import React, { useState, useEffect } from 'react';
import {apiClient} from '../../shared/api/apiClient';
 

// Contrato compativel com o DTO do backend .NET
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

export const CommoditiesPage: React.FC = () => {
  const [data, setData] = useState<CommodityPrice[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedCommodity, setSelectedCommodity] = useState<string>('Todas');

  // Consumo direto do endpoint da API .NET
  useEffect(() => {
    const fetchCommodities = async () => {
      try {
        setLoading(true);
        
       if (selectedCommodity === 'Todas') {
        // Busca Soja e Milho simultaneamente em paralelo
        const [sojaRes, milhoRes] = await Promise.all([
          apiClient.get<CommodityPrice[]>('/commodity-prices/history', { params: { commodity: 'Soja' } }),
          apiClient.get<CommodityPrice[]>('/commodity-prices/history', { params: { commodity: 'Milho' } })
        ]);   
      setData([...sojaRes.data, ...milhoRes.data]);
      }else {

        const response = await apiClient.get<CommodityPrice[]>('/commodity-prices/history', {
          params: { commodity: selectedCommodity }
        });

        setData(response.data);
      }
      } catch (err: unknown) {
        // Tratamento de falhas de conexão/HTTP
        setError(err instanceof Error ? err.message : 'Erro ao carregar dados');
      } finally {
        setLoading(false); // Garante a desativação do spinner
      }
    };

    fetchCommodities();

  }, [selectedCommodity]); // Re-executa quando o filtro de commodity muda

  // Utilitários de formatação local
  const formatCurrency = (val: number) =>
    new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(val);

  const formatDate = (dateStr: string) =>
    new Date(dateStr).toLocaleDateString('pt-BR', { timeZone: 'UTC' });

  // Filtro dinâmico em memória por tipo de commodity
  const filteredData = selectedCommodity === 'Todas' 
    ? data 
    : data.filter(item => item.commodity.toLowerCase() === selectedCommodity.toLowerCase());

  if (loading) {
    return (
      <div className="flex justify-center items-center min-h-[300px]">
        <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-emerald-600"></div>
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
      {/* Header e Filtros */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-gray-200 dark:border-gray-800 pb-4">
        <div>
          <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Cotações de Commodities</h1>
          <p className="text-sm text-gray-500 dark:text-gray-400">Acompanhamento em tempo real via Vanguard API</p>
        </div>

        <div className="flex gap-2">
          {['Todas', 'Soja', 'Milho'].map((item) => (
            <button
              key={item}
              onClick={() => setSelectedCommodity(item)}
              className={`px-4 py-1.5 text-sm font-medium rounded-full transition-colors ${
                selectedCommodity === item
                  ? 'bg-emerald-600 text-white'
                  : 'bg-gray-100 dark:bg-gray-800 text-gray-600 dark:text-gray-300 hover:bg-gray-200'
              }`}
            >
              {item}
            </button>
          ))}
        </div>
      </div>

      {/* Cards de Destaque */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {filteredData.map((item) => {
          const isPositive = (item.dailyVariationPercent ?? 0) >= 0;

          return (
            <div 
              key={item.id} 
              className="p-5 bg-white dark:bg-gray-900 border border-gray-200 dark:border-gray-800 rounded-xl shadow-sm hover:shadow-md transition-shadow"
            >
              <div className="flex justify-between items-start mb-2">
                <div>
                  <span className="text-xs font-semibold tracking-wider text-emerald-600 uppercase">
                    {item.commodity}
                  </span>
                  <h3 className="text-base font-semibold text-gray-800 dark:text-gray-100">
                    {item.market}
                  </h3>
                </div>
                {/* Badge condicional para variação diária */}
                <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-semibold ${
                  isPositive 
                    ? 'bg-emerald-100 text-emerald-800 dark:bg-emerald-950 dark:text-emerald-300' 
                    : 'bg-rose-100 text-rose-800 dark:bg-rose-950 dark:text-rose-300'
                }`}>
                  {isPositive ? '▲ +' : '▼ '}{item.dailyVariationPercent}%
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
          <h2 className="text-lg font-semibold text-gray-800 dark:text-gray-200">Histórico de Coletas</h2>
        </div>
        <div className="overflow-x-auto">
          <table className="w-full text-left text-sm text-gray-600 dark:text-gray-300">
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
              {filteredData.map((row) => (
                <tr key={row.id} className="hover:bg-gray-50 dark:hover:bg-gray-800/50 transition-colors">
                  <td className="px-6 py-4 font-medium whitespace-nowrap">{formatDate(row.referenceDate)}</td>
                  <td className="px-6 py-4">{row.commodity}</td>
                  <td className="px-6 py-4">{row.market}</td>
                  <td className="px-6 py-4 font-semibold text-gray-900 dark:text-white">{formatCurrency(row.priceBrl)}</td>
                  <td className={`px-6 py-4 font-medium ${
                    (row.dailyVariationPercent ?? 0) >= 0 ? 'text-emerald-600' : 'text-rose-600'
                  }`}>
                    {row.dailyVariationPercent}%
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};