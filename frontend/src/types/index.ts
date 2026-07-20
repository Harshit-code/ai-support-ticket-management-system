export type UserRole = 'Admin' | 'Agent' | 'Customer';
export type TicketPriority = 'Low' | 'Medium' | 'High' | 'Critical';
export type TicketStatus = 'Open' | 'InProgress' | 'Resolved' | 'Closed' | 'Cancelled';

export interface User {
  id: number;
  name: string;
  email: string;
  role: UserRole;
}

export interface Comment {
  id: number;
  ticketId: number;
  message: string;
  createdById: number;
  createdBy: User;
  createdAt: string;
}

export interface Ticket {
  id: number;
  title: string;
  description: string;
  priority: TicketPriority;
  status: TicketStatus;
  createdById: number;
  createdBy: User;
  assignedToId: number | null;
  assignedTo: User | null;
  createdAt: string;
  updatedAt: string;
  comments?: Comment[];
}

export interface CreateTicketPayload {
  title: string;
  description: string;
  priority: TicketPriority;
  createdById: number;
  assignedToId?: number | null;
}

export interface UpdateTicketPayload {
  title: string;
  description: string;
  priority: TicketPriority;
  assignedToId?: number | null;
}

export interface ApiError {
  error?: string;
  errors?: Record<string, string[]>;
  title?: string;
}
