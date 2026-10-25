import React, { useState } from 'react';
import type{ CommodityPrice } from './CommoditiesPage';
import {HistoryOutlined} from '@ant-design/icons';


interface CommodityHistoryTableProps {
  data: CommodityPrice[];
  formatDate: (dateStr: string) => string;
  formatCurrency: (val: number) => string;
}

export const CommodityHistoryTable: React.FC<CommodityHistoryTableProps> = ({
  data,
  formatDate,
  formatCurrency,
}) => {
  // Começa recolhido (false) ou expandido conforme sua preferência
  const [isExpanded, setIsExpanded] = useState<boolean>(false);

  return (
    <div className="bg-slate-900 border border-slate-800 rounded-xl overflow-hidden shadow-lg transition-all duration-300">
      {/* Cabeçalho Clicável do Accordion */}
      <button
        type="button"
        onClick={() => setIsExpanded(!isExpanded)}
        className="w-full p-4 bg-slate-900 hover:bg-slate-800/70 transition-colors flex items-center justify-between border-b border-slate-800/80 focus:outline-none focus:ring-1 focus:ring-emerald-500/40"
      >
        <div className="flex items-center gap-3">
          <div className="p-2.5 rounded-lg bg-emerald-950/60 border border-emerald-800/40 text-emerald-400 text-lg flex items-center justify-center">
            <HistoryOutlined />
          </div>
          <div>
            <h2 className="text-base font-semibold text-slate-100">
              Histórico de Coletas
            </h2>
            <p className="text-xs text-slate-400">
              {data.length} registros capturados na base
            </p>
          </div>
        </div>

        <div className="flex items-center gap-3">
          <span className="text-xs text-slate-400 font-medium hidden sm:inline">
            {isExpanded ? 'Ocultar Tabela' : 'Expandir Tabela'}
          </span>
          <div
            className={`p-1.5 rounded-lg bg-slate-800 text-slate-300 transform transition-transform duration-300 ${
              isExpanded ? 'rotate-180 bg-emerald-950 text-emerald-400' : ''
            }`}
          >
            <svg
              className="w-4 h-4"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M19 9l-7 7-7-7"
              />
            </svg>
          </div>
        </div>
      </button>

      {/* Container Deslizante do Accordion */}
      <div
        className={`transition-all duration-300 ease-in-out overflow-hidden ${
          isExpanded ? 'max-h-[700px] opacity-100' : 'max-h-0 opacity-0'
        }`}
      >
        <div className="overflow-x-auto max-h-[600px] overflow-y-auto">
          <table className="w-full text-left text-sm text-slate-300">
            <thead className="bg-slate-950/90 text-xs text-slate-400 uppercase tracking-wider sticky top-0 backdrop-blur-md border-b border-slate-800">
              <tr>
                <th className="px-6 py-3">Data Ref.</th>
                <th className="px-6 py-3">Commodity</th>
                <th className="px-6 py-3">Mercado</th>
                <th className="px-6 py-3">Preço (BRL)</th>
                <th className="px-6 py-3">Variação Diária</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-800/60 bg-slate-900/40">
              {data.map((row) => {
                const isPositive = (row.dailyVariationPercent ?? 0) >= 0;
                return (
                  <tr
                    key={row.id}
                    className="hover:bg-slate-800/40 transition-colors"
                  >
                    <td className="px-6 py-3.5 font-medium whitespace-nowrap text-slate-300">
                      {formatDate(row.referenceDate)}
                    </td>
                    <td className="px-6 py-3.5 text-slate-200 font-semibold">
                      {row.commodity}
                    </td>
                    <td className="px-6 py-3.5 text-slate-400">{row.market}</td>
                    <td className="px-6 py-3.5 font-bold text-white">
                      {formatCurrency(row.priceBrl)}
                    </td>
                    <td
                      className={`px-6 py-3.5 font-medium ${
                        isPositive ? 'text-emerald-400' : 'text-rose-400'
                      }`}
                    >
                      {isPositive ? '▲ +' : '▼ '}
                      {row.dailyVariationPercent}%
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};