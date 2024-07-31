import React, { useEffect, useState } from 'react';
import { Button, Form, Input, Table, message, Modal } from 'antd';
import {
  getAllCategories,
  createCategory,
  deleteCategory,
  updateCategory,
  Category,
} from './Services/categoryService';

const { TextArea } = Input;

const CategoryTable: React.FC = () => {
  const [dataSource, setDataSource] = useState<(Category & { key: string })[]>([]);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [form] = Form.useForm();
  const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);

  useEffect(() => {
    const fetchCategories = async () => {
      const categories = await getAllCategories();
      const categoriesWithKeys = categories.map((category) => ({ ...category, key: category.id }));
      setDataSource(categoriesWithKeys);
    };

    fetchCategories();
  }, []);

  const handleAdd = () => {
    setIsModalVisible(true);
  };

  const handleCancel = () => {
    setIsModalVisible(false);
    form.resetFields();
  };

  const handleOk = async () => {
    try {
      const values = await form.validateFields();
      const newCategory: Omit<Category, 'id'> = {
        name: values.name,
        description: values.description,
        products: [],
      };
      const createdCategory = await createCategory(newCategory);
      setDataSource([...dataSource, { ...createdCategory, key: createdCategory.id }]);
      setIsModalVisible(false);
      form.resetFields();
    } catch (error) {
      console.log('Failed to add category:', error);
    }
  };

  const handleDelete = async () => {
    if (selectedRowKeys.length === 1) {
      const key = selectedRowKeys[0];
      await deleteCategory(key as string);
      setDataSource(dataSource.filter((item) => item.key !== key));
      setSelectedRowKeys([]);
    }
  };

  const handleSave = async (row: Category & { key: string }) => {
    const newData = [...dataSource];
    const index = newData.findIndex((item) => row.key === item.key);
    const item = newData[index];
    newData.splice(index, 1, { ...item, ...row });
    setDataSource(newData);

    const updateData: Category = {
      id: row.key,
      name: row.name,
      description: row.description,
      products: row.products,
    };

    await updateCategory(row.key as string, updateData);
  };

  const columns = [
    {
      title: 'Название',
      dataIndex: 'name',
      editable: true,
    },
    {
      title: 'Описание',
      dataIndex: 'description',
      editable: true,
    },
  ].map((col) => ({
    ...col,
    onCell: (record: Category & { key: string }) => ({
      record,
      editable: col.editable,
      dataIndex: col.dataIndex,
      title: col.title,
      handleSave,
    }),
  }));

  const rowSelection = {
    selectedRowKeys,
    onChange: (keys: React.Key[]) => setSelectedRowKeys(keys),
  };

  return (
    <div>
      <Button onClick={handleAdd} type="primary" style={{ marginBottom: 16, marginRight: 8 }}>
        Добавить категорию
      </Button>
      <Button onClick={handleDelete} type="primary" disabled={selectedRowKeys.length !== 1} style={{ marginBottom: 16 }}>
        Удалить категорию
      </Button>
      <Table
        rowClassName={() => 'editable-row'}
        bordered
        dataSource={dataSource}
        columns={columns}
        rowSelection={rowSelection}
      />
      <Modal
        title="Создать новую категорию"
        visible={isModalVisible}
        onOk={handleOk}
        onCancel={handleCancel}
      >
        <Form form={form} layout="vertical">
          <Form.Item
            name="name"
            label="Название"
            rules={[{ required: true, message: 'Введите название категории' }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="description"
            label="Описание"
            rules={[{ required: true, message: 'Введите описание категории' }]}
          >
            <TextArea />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default CategoryTable;
