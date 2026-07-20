import api from './client';
import type { Ticket, CreateTicketPayload, UpdateTicketPayload, TicketStatus } from '../types';

export const getTickets = (keyword?: string, status?: TicketStatus) => {
  const params: Record<string, string> = {};
  if (keyword?.trim()) params.keyword = keyword.trim();
  if (status)          params.status  = status;
  return api.get<Ticket[]>('/tickets', { params });
};

export const getTicketById = (id: number) =>
  api.get<Ticket>(`/tickets/${id}`);

export const createTicket = (data: CreateTicketPayload) =>
  api.post<Ticket>('/tickets', data);

export const updateTicket = (id: number, data: UpdateTicketPayload) =>
  api.put<Ticket>(`/tickets/${id}`, data);

export const patchTicketStatus = (id: number, newStatus: TicketStatus) =>
  api.patch<Ticket>(`/tickets/${id}/status`, { newStatus });
