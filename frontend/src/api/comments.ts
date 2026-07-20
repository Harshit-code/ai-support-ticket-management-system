import api from './client';
import type { Comment } from '../types';

export const getComments = (ticketId: number) =>
  api.get<Comment[]>(`/tickets/${ticketId}/comments`);

export const createComment = (ticketId: number, message: string, createdById: number) =>
  api.post<Comment>(`/tickets/${ticketId}/comments`, { message, createdById });
