export interface FormattedTrend {
  label: string;
  className: string;
}

export const formatTrend = (trendStr: string): FormattedTrend => {
  const trend = trendStr?.toLowerCase();

  if (trend === 'up') {
    return { label: '▲ ALTA', className: 'text-emerald-400 font-bold' };
  }
  if (trend === 'down') {
    return { label: '▼ BAIXA', className: 'text-rose-400 font-bold' };
  }
  return { label: '➔ ESTÁVEL', className: 'text-slate-300 font-bold' };
};