import axios from 'axios';

const API_URL = 'http://localhost:5030/api/Products'; 

export interface Product {
  id: string;
  name: string;
  description: string;
  price: string;
  categoryId: string;
}

export interface ProductCreateUpdateRequest {
  name: string;
  description: string;
  price: string;
  categoryId: string;
}

export const getAllProducts = async (): Promise<Product[]> => {
  const response = await axios.get<Product[]>(API_URL);
  return response.data;
};

export const createProduct = async (product: ProductCreateUpdateRequest): Promise<Product> => {
  const response = await axios.post<Product>(API_URL, product);
  return response.data;
};

export const updateProduct = async (id: string, product: ProductCreateUpdateRequest): Promise<void> => {
  await axios.put(`${API_URL}/${id}`, product);
};

export const deleteProduct = async (id: string): Promise<void> => {
  await axios.delete(`${API_URL}/${id}`);
};
