import { useState, useEffect, useCallback } from 'react';
import { Link } from 'react-router-dom';
import { getTickets } from '../api/tickets';
import type { Ticket, TicketStatus } from '../types';
import StatusBadge from '../components/StatusBadge';
import PriorityBadge from '../components/PriorityBadge';
import LoadingSpinner from '../components/LoadingSpinner';
import ErrorMessage from '../components/ErrorMessage';
import EmptyState from '../components/EmptyState';

const ALL_STATUSES: TicketStatus[] = ['Open', 'InProgress', 'Resolved', 'Closed', 'Cancelled'];
const STATUS_LABELS: Record<TicketStatus, string> = {
  Open: 'Open', InProgress: 'In Progress', Resolved: 'Resolved',
  Closed: 'Closed', Cancelled: 'Cancelled',
};

export default function TicketListPage() {
  const [tickets, setTickets]   = useState<Ticket[]>([]);
  const [loading, setLoading]   = useState(true);
  const [error, setError]       = useState<string | null>(null);
  const [keyword, setKeyword]   = useState('');
  const [status, setStatus]     = useState<TicketStatus | ''>('');
  const [search, setSearch]     = useState('');

  const fetchTickets = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const res = await getTickets(search || undefined, status || undefined);
      setTickets(res.data);
    } catch {
      setError('Failed to load tickets. Check that the API is running.');
    } finally {
      setLoading(false);
    }
  }, [search, status]);

  useEffect(() => { fetchTickets(); }, [fetchTickets]);

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    setSearch(keyword);
  };

  return (
    <div className="max-w-5xl mx-auto px-4 py-8">
      {/* Header */}
      <div className="flex items-center justify-between mb-6">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Support Tickets</h1>
          {!loading && !error && (
            <p className="text-sm text-gray-500 mt-1">{tickets.length} ticket{tickets.length !== 1 ? 's' : ''}</p>
          )}
        </div>
        <Link
          to="/tickets/new"
          className="px-4 py-2 bg-blue-600 text-white text-sm font-medium rounded-lg hover:bg-blue-700 transition-colors"
        >
          + New Ticket
        </Link>
      </div>

      {/* Filters */}
      <div className="flex flex-col sm:flex-row gap-3 mb-6">
        <form onSubmit={handleSearch} className="flex flex-1 gap-2">
          <input
            type="text"
            value={keyword}
            onChange={e => setKeyword(e.target.value)}
            placeholder="Search title or description…"
            className="flex-1 px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
          <button
            type="submit"
            className="px-4 py-2 bg-gray-800 text-white text-sm rounded-lg hover:bg-gray-900 transition-colors"
          >
            Search
          </button>
          {(search || status) && (
            <button
              type="button"
              onClick={() => { setKeyword(''); setSearch(''); setStatus(''); }}
              className="px-3 py-2 text-sm text-gray-500 border border-gray-300 rounded-lg hover:bg-gray-50"
            >
              Clear
            </button>
          )}
        </form>
        <select
          value={status}
          onChange={e => setStatus(e.target.value as TicketStatus | '')}
          className="px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
        >
          <option value="">All statuses</option>
          {ALL_STATUSES.map(s => (
            <option key={s} value={s}>{STATUS_LABELS[s]}</option>
          ))}
        </select>
      </div>

      {/* Content */}
      {loading && <LoadingSpinner message="Loading tickets…" />}
      {error   && <ErrorMessage message={error} onRetry={fetchTickets} />}
      {!loading && !error && tickets.length === 0 && (
        <EmptyState
          title="No tickets found"
          subtitle={search || status ? 'Try adjusting your search or filter.' : 'Create the first ticket to get started.'}
        />
      )}
      {!loading && !error && tickets.length > 0 && (
        <div className="space-y-3">
          {tickets.map(ticket => (
            <Link
              key={ticket.id}
              to={`/tickets/${ticket.id}`}
              className="block bg-white border border-gray-200 rounded-xl p-5 hover:border-blue-400 hover:shadow-sm transition-all"
            >
              <div className="flex items-start justify-between gap-4">
                <div className="flex-1 min-w-0">
                  <div className="flex items-center gap-2 mb-1">
                    <span className="text-xs font-mono text-gray-400">#{ticket.id}</span>
                    <h2 className="text-sm font-semibold text-gray-900 truncate">{ticket.title}</h2>
                  </div>
                  <p className="text-sm text-gray-500 line-clamp-2">{ticket.description}</p>
                </div>
                <div className="flex flex-col items-end gap-1.5 shrink-0">
                  <StatusBadge status={ticket.status} />
                  <PriorityBadge priority={ticket.priority} />
                </div>
              </div>
              <div className="flex items-center gap-4 mt-3 text-xs text-gray-400">
                <span>By {ticket.createdBy?.name ?? '—'}</span>
                {ticket.assignedTo && <span>Assigned to {ticket.assignedTo.name}</span>}
                <span className="ml-auto">{new Date(ticket.createdAt).toLocaleDateString()}</span>
              </div>
            </Link>
          ))}
        </div>
      )}
    </div>
  );
}
