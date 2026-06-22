import { Card, Tag } from "antd";
import type { ReactNode } from "react";

type StatusCardProps = {
  title: string;
  value: string;
  description: string;
  icon: ReactNode;
  status: "success" | "warning" | "error" | "info";
};

const statusConfig = {
  success: {
    label: "Saudável",
    color: "success",
  },
  warning: {
    label: "Atenção",
    color: "warning",
  },
  error: {
    label: "Crítico",
    color: "error",
  },
  info: {
    label: "Informativo",
    color: "processing",
  },
};

export function StatusCard({
  title,
  value,
  description,
  icon,
  status,
}: StatusCardProps) {
  const currentStatus = statusConfig[status];

  return (
    <Card className="border border-slate-800 bg-slate-900" size="small">
      <div className="flex items-start justify-between gap-4">
        <div>
          <p className="mb-2 text-sm text-slate-400">{title}</p>

          <h2 className="mb-1 text-2xl font-bold text-slate-100">{value}</h2>

          <p className="mb-0 text-sm text-slate-500">{description}</p>
        </div>

        <div className="flex h-11 w-11 items-center justify-center rounded-xl bg-cyan-500/10 text-xl text-cyan-400">
          {icon}
        </div>
      </div>

      <div className="mt-2">
        <Tag color={currentStatus.color}>{currentStatus.label}</Tag>
      </div>
    </Card>
  );
}