import { useState, useEffect, useCallback } from 'react';
import { useParams, Link } from 'react-router-dom';
import axios from 'axios';
import { getTicketById, patchTicketStatus } from '../api/tickets';
import { createComment } from '../api/comments';
import type { Ticket, TicketStatus, Comment } from '../types';
import { getAllowedTransitions, isTerminal } from '../utils/statusTransitions';
import StatusBadge from '../components/StatusBadge';
import PriorityBadge from '../components/PriorityBadge';
import LoadingSpinner from '../components/LoadingSpinner';
import ErrorMessage from '../components/ErrorMessage';
import EmptyState from '../components/EmptyState';

const STATUS_LABELS: Record<TicketStatus, string> = {
  Open: 'Open', InProgress: 'In Progress', Resolved: 'Resolved',
  Closed: 'Closed', Cancelled: 'Cancelled',
};

interface TransitionError {
  message: string;
  allowed: string[];
}

export default function TicketDetailPage() {
  const { id } = useParams<{ id: string }>();
  const ticketId = Number(id);

  const [ticket, setTicket]           = useState<Ticket | null>(null);
  const [loading, setLoading]         = useState(true);
  const [error, setError]             = useState<string | null>(null);
  const [transitioning, setTransitioning] = useState(false);
  const [transitionError, setTransitionError] = useState<TransitionError | null>(null);

  const [commentText, setCommentText] = useState('');
  const [commentUserId, setCommentUserId] = useState<number>(1);
  const [submittingComment, setSubmittingComment] = useState(false);
  const [commentError, setCommentError] = useState<string | null>(null);

  const fetchTicket = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const res = await getTicketById(ticketId);
      setTicket(res.data);
    } catch (err: unknown) {
      const status = (err as { response?: { status?: number } })?.response?.status;
      setError(status === 404 ? `Ticket #${ticketId} not found.` : 'Failed to load ticket.');
    } finally {
      setLoading(false);
    }
  }, [ticketId]);

  useEffect(() => { fetchTicket(); }, [fetchTicket]);

  const handleStatusChange = async (newStatus: TicketStatus) => {
    if (!ticket) return;
    setTransitioning(true);
    setTransitionError(null);
    try {
      const res = await patchTicketStatus(ticket.id, newStatus);
      setTicket(res.data);
    } catch (err) {
      if (axios.isAxiosError(err) && err.response?.data) {
        const data = err.response.data as { error?: string; allowed?: string[] };
        setTransitionError({
          message: data.error ?? 'Transition failed.',
          allowed: data.allowed ?? [],
        });
      } else {
        setTransitionError({ message: 'Failed to update status. Please try again.', allowed: [] });
      }
    } finally {
      setTransitioning(false);
    }
  };

  const handleAddComment = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!ticket || !commentText.trim()) return;
    setSubmittingComment(true);
    setCommentError(null);
    try {
      const res = await createComment(ticket.id, commentText.trim(), commentUserId);
      setTicket(prev => prev ? {
        ...prev,
        comments: [...(prev.comments ?? []), res.data as unknown as Comment],
      } : prev);
      setCommentText('');
    } catch {
      setCommentError('Failed to add comment. Please try again.');
    } finally {
      setSubmittingComment(false);
    }
  };

  if (loading) return <div className="max-w-3xl mx-auto px-4 py-8"><LoadingSpinner message="Loading ticket…" /></div>;
  if (error)   return <div className="max-w-3xl mx-auto px-4 py-8"><ErrorMessage message={error} onRetry={fetchTicket} /></div>;
  if (!ticket) return null;

  const allowedNext = getAllowedTransitions(ticket.status);
  const terminal    = isTerminal(ticket.status);
  const comments    = ticket.comments ?? [];

  return (
    <div className="max-w-3xl mx-auto px-4 py-8">
      {/* Back */}
      <Link to="/" className="inline-flex items-center text-sm text-blue-600 hover:underline mb-6">
        ← Back to tickets
      </Link>

      {/* Ticket header */}
      <div className="bg-white border border-gray-200 rounded-xl p-6 mb-6">
        <div className="flex items-start justify-between gap-4 mb-4">
          <div>
            <span className="text-xs font-mono text-gray-400 block mb-1">#{ticket.id}</span>
            <h1 className="text-xl font-bold text-gray-900">{ticket.title}</h1>
          </div>
          <div className="flex flex-col items-end gap-1.5 shrink-0">
            <StatusBadge status={ticket.status} />
            <PriorityBadge priority={ticket.priority} />
          </div>
        </div>

        <p className="text-gray-700 text-sm leading-relaxed mb-5">{ticket.description}</p>

        <div className="grid grid-cols-2 gap-3 text-sm text-gray-600 border-t pt-4">
          <div><span className="text-gray-400 text-xs uppercase tracking-wide">Created by</span><p className="font-medium mt-0.5">{ticket.createdBy?.name ?? '—'}</p></div>
          <div><span className="text-gray-400 text-xs uppercase tracking-wide">Assigned to</span><p className="font-medium mt-0.5">{ticket.assignedTo?.name ?? 'Unassigned'}</p></div>
          <div><span className="text-gray-400 text-xs uppercase tracking-wide">Created</span><p className="font-medium mt-0.5">{new Date(ticket.createdAt).toLocaleString()}</p></div>
          <div><span className="text-gray-400 text-xs uppercase tracking-wide">Updated</span><p className="font-medium mt-0.5">{new Date(ticket.updatedAt).toLocaleString()}</p></div>
        </div>

        {/* Status transition */}
        <div className="mt-5 pt-4 border-t">
          <p className="text-xs font-medium text-gray-500 uppercase tracking-wide mb-2">Change status</p>
          {terminal ? (
            <p className="text-sm text-gray-400 italic">This ticket is in a terminal state and cannot be moved further.</p>
          ) : (
            <div className="flex items-center gap-3 flex-wrap">
              {allowedNext.map(next => (
                <button
                  key={next}
                  onClick={() => handleStatusChange(next)}
                  disabled={transitioning}
                  className="px-4 py-2 text-sm font-medium border border-gray-300 rounded-lg hover:bg-gray-50 disabled:opacity-50 transition-colors"
                >
                  {transitioning ? '…' : `→ ${STATUS_LABELS[next]}`}
                </button>
              ))}
            </div>
          )}
          {transitionError && (
            <div className="mt-3 p-3 bg-red-50 border border-red-200 rounded-lg space-y-2">
              <p className="text-sm text-red-700 font-medium">{transitionError.message}</p>
              {transitionError.allowed.length > 0 && (
                <div className="flex items-center gap-2 flex-wrap">
                  <span className="text-xs text-red-500">Valid transitions:</span>
                  {transitionError.allowed.map(s => (
                    <span key={s} className="text-xs px-2 py-0.5 bg-white border border-red-200 rounded text-red-600 font-medium">
                      {STATUS_LABELS[s as TicketStatus] ?? s}
                    </span>
                  ))}
                </div>
              )}
            </div>
          )}
        </div>
      </div>

      {/* Comments */}
      <div className="bg-white border border-gray-200 rounded-xl p-6">
        <h2 className="text-base font-semibold text-gray-900 mb-4">
          Comments {comments.length > 0 && <span className="text-gray-400 font-normal">({comments.length})</span>}
        </h2>

        {comments.length === 0 ? (
          <EmptyState title="No comments yet" subtitle="Add the first comment below." />
        ) : (
          <div className="space-y-4 mb-6">
            {comments.map(c => (
              <div key={c.id} className="flex gap-3">
                <div className="w-8 h-8 rounded-full bg-blue-100 text-blue-700 flex items-center justify-center text-sm font-bold shrink-0">
                  {c.createdBy?.name?.[0] ?? '?'}
                </div>
                <div className="flex-1">
                  <div className="flex items-baseline gap-2 mb-1">
                    <span className="text-sm font-medium text-gray-800">{c.createdBy?.name ?? '—'}</span>
                    <span className="text-xs text-gray-400">{new Date(c.createdAt).toLocaleString()}</span>
                  </div>
                  <p className="text-sm text-gray-700 leading-relaxed whitespace-pre-wrap">{c.message}</p>
                </div>
              </div>
            ))}
          </div>
        )}

        {/* Add comment form */}
        <form onSubmit={handleAddComment} className="border-t pt-4 space-y-3">
          <textarea
            value={commentText}
            onChange={e => setCommentText(e.target.value)}
            placeholder="Write a comment…"
            rows={3}
            className="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 resize-none"
          />
          <div className="flex items-center justify-between gap-3">
            <div className="flex items-center gap-2 text-sm text-gray-500">
              <span>Commenting as user ID</span>
              <input
                type="number"
                value={commentUserId}
                onChange={e => setCommentUserId(Number(e.target.value))}
                min={1}
                className="w-16 px-2 py-1 border border-gray-300 rounded text-sm focus:outline-none focus:ring-1 focus:ring-blue-400"
              />
            </div>
            <button
              type="submit"
              disabled={submittingComment || !commentText.trim()}
              className="px-4 py-2 bg-blue-600 text-white text-sm font-medium rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
            >
              {submittingComment ? 'Adding…' : 'Add Comment'}
            </button>
          </div>
          {commentError && <p className="text-red-600 text-sm">{commentError}</p>}
        </form>
      </div>
    </div>
  );
}
