import type { TicketStatus } from '../types';

// Mirrors TicketStatusTransitions.cs exactly — keep in sync with the backend.
export const STATUS_TRANSITIONS: Record<TicketStatus, TicketStatus[]> = {
  Open:       ['InProgress', 'Cancelled'],
  InProgress: ['Resolved',   'Cancelled'],
  Resolved:   ['Closed'],
  Closed:     [],
  Cancelled:  [],
};

export function getAllowedTransitions(current: TicketStatus): TicketStatus[] {
  return STATUS_TRANSITIONS[current] ?? [];
}

export function isTerminal(status: TicketStatus): boolean {
  return STATUS_TRANSITIONS[status].length === 0;
}
