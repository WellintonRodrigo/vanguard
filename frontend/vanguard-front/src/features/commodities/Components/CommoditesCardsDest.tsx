import React, { useMemo, useState } from 'react';
import { AppstoreOutlined, DownOutlined } from '@ant-design/icons';
import { type CommodityPrice } from './CommoditiesPage';

interface CommodityKpiGridProps {
  data: CommodityPrice[];
  formatCurrency: (val: number) => string;
  formatDate: (dateStr: string) => string;
}

export const CommoditesCardsDest: React.FC<CommodityKpiGridProps> = ({
  data,
  formatCurrency,
  formatDate,
}) => {
  const [isExpanded, setIsExpanded] = useState<boolean>(true);

  // MODO 1: Filtra apenas a referência mais recente de cada mercado/commodity
  const latestPrices = useMemo(() => {
    const map = new Map<string, CommodityPrice>();

    data.forEach((item) => {
      const key = `${item.commodity}-${item.market}`;
      const existing = map.get(key);

      if (!existing) {
        map.set(key, item);
      } else {
        const itemDate = new Date(item.referenceDate).getTime();
        const existingDate = new Date(existing.referenceDate).getTime();
        if (itemDate > existingDate) {
          map.set(key, item);
        }
      }
    });

    return Array.from(map.values());
  }, [data]);

  return (
    <div className="bg-slate-900 border border-slate-800 rounded-xl overflow-hidden shadow-lg transition-all duration-300 mb-6">
      {/* MODO 2: Cabeçalho Accordion */}
      <button
        type="button"
        onClick={() => setIsExpanded(!isExpanded)}
        className="w-full p-4 bg-slate-900 hover:bg-slate-800/70 transition-colors flex items-center justify-between border-b border-slate-800/80 focus:outline-none"
      >
        <div className="flex items-center gap-3">
          <div className="p-2.5 rounded-lg bg-emerald-950/60 border border-emerald-800/40 text-emerald-400 text-lg flex items-center justify-center">
            <AppstoreOutlined />
          </div>
          <div className="text-left">
            <h2 className="text-base font-semibold text-slate-100">
              Cotações em Tempo Real
            </h2>
            <p className="text-xs text-slate-400">
              {latestPrices.length} praças monitoradas (Última referência)
            </p>
          </div>
        </div>

        <div className="flex items-center gap-3">
          <span className="text-xs text-slate-400 font-medium hidden sm:inline">
            {isExpanded ? 'Recolher Cards' : 'Expandir Cards'}
          </span>
          <div
            className={`p-1.5 rounded-lg bg-slate-800 text-slate-300 transform transition-transform duration-300 text-xs flex items-center justify-center ${
              isExpanded ? 'rotate-180 bg-emerald-950 text-emerald-400' : ''
            }`}
          >
            <DownOutlined />
          </div>
        </div>
      </button>

      {/* Grid de Cards Sanfonado */}
      <div
        className={`transition-all duration-300 ease-in-out overflow-hidden ${
          isExpanded ? 'max-h-[1200px] opacity-100 p-5' : 'max-h-0 opacity-0 p-0'
        }`}
      >
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {latestPrices.map((item) => {
            const isPositive = (item.dailyVariationPercent ?? 0) >= 0;
            return (
              <div
                key={item.id}
                className="bg-slate-950/60 border border-slate-800/80 rounded-xl p-4 shadow-md hover:border-slate-700 transition-all flex flex-col justify-between"
              >
                <div>
                  <div className="flex items-center justify-between mb-1">
                    <span className="text-[11px] font-bold tracking-wider text-emerald-400 uppercase">
                      {item.commodity}
                    </span>
                    <span
                      className={`text-xs font-semibold px-2 py-0.5 rounded ${
                        isPositive
                          ? 'bg-emerald-950/80 text-emerald-400 border border-emerald-800/50'
                          : 'bg-rose-950/80 text-rose-400 border border-rose-800/50'
                      }`}
                    >
                      {isPositive ? '▲+' : '▼'}
                      {item.dailyVariationPercent}%
                    </span>
                  </div>

                  <h3 className="text-sm font-semibold text-slate-200 truncate">
                    {item.market}
                  </h3>

                  <div className="mt-3 flex items-baseline gap-2">
                    <span className="text-2xl font-extrabold text-white">
                      {formatCurrency(item.priceBrl)}
                    </span>
                    <span className="text-xs text-slate-500 font-medium">
                      / {item.unit || 'Saca 60kg'}
                    </span>
                  </div>
                </div>

                <div className="mt-4 pt-3 border-t border-slate-800/60 flex justify-between items-center text-[11px] text-slate-400">
                  <span>Fonte: {item.source}</span>
                  <span>Ref: {formatDate(item.referenceDate)}</span>
                </div>
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
};