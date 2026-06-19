import { createBrowserRouter } from "react-router-dom";

import { MainLayout } from "../shared/layouts/MainLayout";

import { DashboardPage } from "../features/dashboard/DashboardPage";
import { CommoditiesPage } from "../features/commodities/CommoditiesPage";
import { WeatherPage } from "../features/weather/WeatherPage";
import { FreshnessPage } from "../features/freshness/FreshnessPage";
import { HealthPage } from "../features/health/HealthPage";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <MainLayout />,
    children: [
      {
        index: true,
        element: <DashboardPage />,
      },
      {
        path: "commodities",
        element: <CommoditiesPage />,
      },
      {
        path: "weather",
        element: <WeatherPage />,
      },
      {
        path: "freshness",
        element: <FreshnessPage />,
      },
      {
        path: "health",
        element: <HealthPage />,
      },
    ],
  },
]);