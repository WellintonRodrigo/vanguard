import React, { useMemo } from 'react';
import  { type CommodityPrice }  from './CommoditiesPage';

interface CommodityKPIsProps {
  data?: CommodityPrice[];
}

export const CommodityKPIs: React.FC<CommodityKPIsProps> = ({ data = [] }) => {
  // Memoiza os cálculos para recarregar apenas quando o array 'data' for alterado
  const stats = useMemo(() => {
    if (!data || data.length === 0) return null;

    // Isola os valores de preço da API em um array numérico
    const prices = data.map((item) => item.priceBrl);

    return {
      maxPrice: Math.max(...prices), // Pega o maior preço da lista
      minPrice: Math.min(...prices), // Pega o menor preço da lista
      avgPrice: prices.reduce((acc, curr) => acc + curr, 0) / prices.length, // Média das cotações
      totalCount: data.length, // Total de registros analisados
    };
  }, [data]);

  if (!stats) return null;

  return (
    <div className="grid grid-cols-1 sm:grid-cols-3 gap-4 mb-8">
      {/* Card Maior Cotação */}
      <div className="bg-slate-900/80 border border-slate-800 p-4 rounded-xl shadow-sm">
        <span className="text-xs uppercase text-slate-400 font-semibold tracking-wider">Maior Cotação</span>
        <p className="text-2xl font-bold text-emerald-400 mt-1">
          R$ {stats.maxPrice.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
        </p>
      </div>

      {/* Card Menor Cotação */}
      <div className="bg-slate-900/80 border border-slate-800 p-4 rounded-xl shadow-sm">
        <span className="text-xs uppercase text-slate-400 font-semibold tracking-wider">Menor Cotação</span>
        <p className="text-2xl font-bold text-rose-400 mt-1">
          R$ {stats.minPrice.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
        </p>
      </div>

      {/* Card Média Geral */}
      <div className="bg-slate-900/80 border border-slate-800 p-4 rounded-xl shadow-sm">
        <span className="text-xs uppercase text-slate-400 font-semibold tracking-wider">Média do Período</span>
        <p className="text-2xl font-bold text-sky-400 mt-1">
          R$ {stats.avgPrice.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
        </p>
      </div>
    </div>
  );
};