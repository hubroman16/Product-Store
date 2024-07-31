"use client"
import React, { useState, ReactNode } from 'react';
import { Layout, Menu, Button } from 'antd';
import {
  TagFilled,
  AppleFilled,
  ShopOutlined,
  MenuUnfoldOutlined,
  MenuFoldOutlined
} from '@ant-design/icons';
import 'antd/dist/reset.css';
import './globals.css';
import EditableTable from './EditableTable';
import EditableCategoryTable from './EditableCategoryTable';

const { Header, Sider, Content } = Layout;

interface LayoutProps {
  children: ReactNode;
}

const MyLayout: React.FC<LayoutProps> = ({ children }) => {
  const [collapsed, setCollapsed] = useState(false);
  const [selectedMenuKey, setSelectedMenuKey] = useState('1');

  const handleMenuClick = ({ key }: { key: string }) => {
    setSelectedMenuKey(key);
  };

  const renderContent = () => {
    switch (selectedMenuKey) {
      case '1':
        return <EditableTable />;
      case '2':
        return <EditableCategoryTable/>;
      default:
        return <div>Продукты</div>;
    }
  };

  return (
    <Layout style={{ height: '100vh' }}>
      <Sider trigger={null} collapsible collapsed={collapsed}>
        <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', padding: '20px' }}>
          <ShopOutlined style={{ fontSize: '45px', color: '#ffffff' }}/>
        </div>
        <Menu theme="dark" mode="inline" defaultSelectedKeys={['1']} onClick={handleMenuClick}>
          <Menu.Item key="1" icon={<AppleFilled />}>
            Продукты
          </Menu.Item>
          <Menu.Item key="2" icon={<TagFilled />}>
            Категории
          </Menu.Item>
        </Menu>
      </Sider>
      <Layout>
        <Header style={{ padding: 0, background: '#fff', textAlign: 'center' }}>
          <Button
            type="text"
            icon={collapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
            onClick={() => setCollapsed(!collapsed)}
            style={{
              fontSize: '16px',
              width: 64,
              height: 64,
              position: 'absolute',
              left: 0,
            }}
          />
          <span style={{ fontSize: '24px' }}>Product Store</span>
        </Header>
        <Content style={{ margin: '24px 16px', padding: 24, background: '#fff', borderRadius: '8px' }}>
          {renderContent()}
        </Content>
      </Layout>
    </Layout>
  );
};

export default MyLayout;

