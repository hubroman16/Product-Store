import React, { useEffect, useState } from 'react';
import { Button, Form, Input, Table, Modal, Select } from 'antd';
import { getAllProducts, createProduct, updateProduct, deleteProduct, Product } from './Services/productService';
import { getAllCategories, Category } from './Services/categoryService';

const { Option } = Select;
const { TextArea } = Input;

const EditableTable: React.FC = () => {
  const [dataSource, setDataSource] = useState<(Product & { key: string })[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [form] = Form.useForm();
  const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);

  useEffect(() => {
    const fetchProducts = async () => {
      const products = await getAllProducts();
      const productsWithKeys = products.map((product) => ({ ...product, key: product.id }));
      setDataSource(productsWithKeys);
    };

    const fetchCategories = async () => {
      const categories = await getAllCategories();
      setCategories(categories);
    };

    fetchProducts();
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
      const newProduct: Omit<Product, 'id'> = {
        name: values.name,
        description: values.description,
        price: values.price,
        categoryId: values.categoryId,
      };
      const createdProduct = await createProduct(newProduct);
      setDataSource([...dataSource, { ...createdProduct, key: createdProduct.id }]);
      setIsModalVisible(false);
      form.resetFields();
    } catch (error) {
      console.log('Failed to add product:', error);
    }
  };

  const handleDelete = async () => {
    if (selectedRowKeys.length === 1) {
      const key = selectedRowKeys[0];
      await deleteProduct(key as string);
      setDataSource(dataSource.filter((item) => item.key !== key));
      setSelectedRowKeys([]);
    }
  };

  const handleSave = async (row: Product & { key: string }) => {
    const newData = [...dataSource];
    const index = newData.findIndex((item) => row.key === item.key);
    const item = newData[index];
    newData.splice(index, 1, { ...item, ...row });
    setDataSource(newData);

    const updateData: Product = {
      id: row.key,
      name: row.name,
      description: row.description,
      price: row.price,
      categoryId: row.categoryId,
    };

    await updateProduct(row.key as string, updateData);
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
    {
      title: 'Цена(руб)',
      dataIndex: 'price',
      editable: true,
    },
    {
      title: 'Категория',
      dataIndex: 'categoryId',
      editable: true,
      render: (categoryId: string) => {
        const category = categories.find((cat) => cat.id === categoryId);
        return category ? category.name : '';
      },
    },
  ].map((col) => ({
    ...col,
    onCell: (record: Product & { key: string }) => ({
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
        Добавить продукт
      </Button>
      <Button onClick={handleDelete} type="primary" disabled={selectedRowKeys.length !== 1} style={{ marginBottom: 16 }}>
        Удалить продукт
      </Button>
      <Table
        rowClassName={() => 'editable-row'}
        bordered
        dataSource={dataSource}
        columns={columns}
        rowSelection={rowSelection}
      />
      <Modal
        title="Создать новый продукт"
        visible={isModalVisible}
        onOk={handleOk}
        onCancel={handleCancel}
      >
        <Form form={form} layout="vertical">
          <Form.Item
            name="name"
            label="Название"
            rules={[{ required: true, message: 'Введите название продукта' }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="description"
            label="Описание"
            rules={[{ required: true, message: 'Введите описание продукта' }]}
          >
            <TextArea />
          </Form.Item>
          <Form.Item
            name="price"
            label="Цена(руб)"
            rules={[{ required: true, message: 'Введите цену продукта' }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="categoryId"
            label="Категория"
            rules={[{ required: true, message: 'Выберите категорию продукта' }]}
          >
            <Select>
              {categories.map((category) => (
                <Option key={category.id} value={category.id}>
                  {category.name}
                </Option>
              ))}
            </Select>
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default EditableTable;
