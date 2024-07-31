import axios from 'axios';
import { Product } from './productService';

const API_URL = 'http://localhost:5030/Category'; // Замените на ваш URL API

export interface Category {
  id: string;
  name: string;
  description: string;
  products: Product[];
}

export interface CategoryWithKey extends Category {
    key: string;
  }

export interface CategoryCreateUpdateRequest {
  name: string;
  description: string;
}

export const getAllCategories = async (): Promise<Category[]> => {
  const response = await axios.get<Category[]>(API_URL);
  return response.data;
};

export const getCategoryById = async (id: string): Promise<Category> => {
  const response = await axios.get<Category>(`${API_URL}/${id}`);
  return response.data;
};

export const createCategory = async (category: CategoryCreateUpdateRequest): Promise<Category> => {
  const response = await axios.post<Category>(API_URL, category);
  return response.data;
};

export const updateCategory = async (id: string, category: CategoryCreateUpdateRequest): Promise<void> => {
  await axios.put(`${API_URL}/${id}`, category);
};

export const deleteCategory = async (id: string): Promise<void> => {
  await axios.delete(`${API_URL}/${id}`);
};
