import {
  CloudOutlined,
  DashboardOutlined,
  DatabaseOutlined,
  HeartOutlined,
  SyncOutlined,
} from "@ant-design/icons";

import { Layout, Menu } from "antd";
import { Outlet, useNavigate } from "react-router-dom";

const { Header, Sider, Content } = Layout;

export function MainLayout() {
  const navigate = useNavigate();

  const handleMenuClick = ({ key }: { key: string }) => {
    navigate(key);
  };

  return (
    <Layout className="min-h-screen bg-slate-950">
      <Sider theme="dark" width={260} className="bg-slate-950">
        <div className="px-6 py-5 text-xl font-bold text-slate-100">
          🚀 Vanguard
        </div>

        <Menu
          theme="dark"
          mode="inline"
          defaultSelectedKeys={["/"]}
          onClick={handleMenuClick}
          items={[
            { key: "/", icon: <DashboardOutlined />, label: "Dashboard" },
            {
              key: "/commodities",
              icon: <DatabaseOutlined />,
              label: "Commodities",
            },
            { key: "/weather", icon: <CloudOutlined />, label: "Weather" },
            { key: "/freshness", icon: <SyncOutlined />, label: "Freshness" },
            { key: "/health", icon: <HeartOutlined />, label: "Health" },
          ]}
        />
      </Sider>

      <Layout className="bg-slate-900">
        <Header className="border-b border-slate-800 bg-slate-950 px-6 text-slate-100">
          Vanguard Intelligence Center
        </Header>

        <Content className="p-6">
          <Outlet />
        </Content>
      </Layout>
    </Layout>
  );
}