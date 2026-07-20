import api from './client';
import type { User } from '../types';

export const getUsers = () => api.get<User[]>('/users');
