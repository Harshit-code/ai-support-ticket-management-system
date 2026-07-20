import type { TicketStatus } from '../types';

const styles: Record<TicketStatus, string> = {
  Open:       'bg-blue-100 text-blue-800',
  InProgress: 'bg-amber-100 text-amber-800',
  Resolved:   'bg-green-100 text-green-800',
  Closed:     'bg-gray-100 text-gray-600',
  Cancelled:  'bg-red-100 text-red-700',
};

const labels: Record<TicketStatus, string> = {
  Open:       'Open',
  InProgress: 'In Progress',
  Resolved:   'Resolved',
  Closed:     'Closed',
  Cancelled:  'Cancelled',
};

export default function StatusBadge({ status }: { status: TicketStatus }) {
  return (
    <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${styles[status]}`}>
      {labels[status]}
    </span>
  );
}
