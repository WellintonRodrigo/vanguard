import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import "antd/dist/reset.css"
import { ConfigProvider, theme } from "antd";
import App from "./app/App"
import "./index.css";


createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ConfigProvider
      theme={{
         algorithm: theme.darkAlgorithm,
         token: {
          colorPrimary: "#06B6D4",
          colorSuccess: "#22C55E",
          colorWarning: "#F59E0B",
          colorError: "#EF4444",
          colorBgBase: "#020617",
          colorTextBase: "#F8FAFC",
          borderRadius: 10,
        },
      }}
    > 
    <App />
    </ConfigProvider>
  </StrictMode>,
)
