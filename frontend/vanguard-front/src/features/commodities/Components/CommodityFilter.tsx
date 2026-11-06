import React, { useState, useMemo } from 'react';

interface CommodityItem {
  commodity: string;
  [key: string]: unknown;
}

interface CommodityFilterProps {
  data: CommodityItem[];
  onFilteredDataChange: (filtered: CommodityItem[]) => void;
}

export const CommodityFilter: React.FC<CommodityFilterProps> = ({
  data,
  onFilteredDataChange,
}) => {
  const [selectedCommodity, setSelectedCommodity] = useState<string>('Todas');

  // 1. Extrai dinamicamente as commodities existentes no banco para o dropdown
  const availableCategories = useMemo(() => {
    if (!data || data.length === 0) return ['Todas'];
    const categories = Array.from(new Set(data.map((item) => item.commodity))).filter(Boolean);
    return ['Todas', ...categories.sort()];
  }, [data]);

  // 2. Filtro instantâneo em memória (0ms) ao trocar a opção no <select>
  const filteredData = useMemo(() => {
    if (selectedCommodity === 'Todas') {
      return data;
    }
    return data.filter(
      (item) => item.commodity.toLowerCase() === selectedCommodity.toLowerCase()
    );
  }, [data, selectedCommodity]);

  // 3. Notifica a página pai sempre que os dados filtrados mudam
  React.useEffect(() => {
    onFilteredDataChange(filteredData);
  }, [filteredData, onFilteredDataChange]);

  return (
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
  );
};