import { useState, useEffect } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { createTicket } from '../api/tickets';
import { getUsers } from '../api/users';
import type { User, TicketPriority } from '../types';

const PRIORITIES: TicketPriority[] = ['Low', 'Medium', 'High', 'Critical'];

interface FormErrors {
  title?: string;
  description?: string;
  priority?: string;
  createdById?: string;
}

export default function CreateTicketPage() {
  const navigate = useNavigate();

  const [users, setUsers]       = useState<User[]>([]);
  const [title, setTitle]       = useState('');
  const [description, setDescription] = useState('');
  const [priority, setPriority] = useState<TicketPriority>('Medium');
  const [createdById, setCreatedById]   = useState<number>(0);
  const [assignedToId, setAssignedToId] = useState<number | ''>('');

  const [errors, setErrors]     = useState<FormErrors>({});
  const [submitError, setSubmitError] = useState<string | null>(null);
  const [submitting, setSubmitting]   = useState(false);
  const [loadingUsers, setLoadingUsers] = useState(true);

  useEffect(() => {
    getUsers()
      .then(res => {
        setUsers(res.data);
        if (res.data.length > 0) setCreatedById(res.data[0].id);
      })
      .catch(() => { /* non-fatal — user can still type an id */ })
      .finally(() => setLoadingUsers(false));
  }, []);

  const validate = (): boolean => {
    const errs: FormErrors = {};
    if (!title.trim())        errs.title       = 'Title is required.';
    if (!description.trim())  errs.description = 'Description is required.';
    if (!createdById)         errs.createdById = 'Please select who is creating this ticket.';
    setErrors(errs);
    return Object.keys(errs).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!validate()) return;
    setSubmitting(true);
    setSubmitError(null);
    try {
      const res = await createTicket({
        title:       title.trim(),
        description: description.trim(),
        priority,
        createdById,
        assignedToId: assignedToId !== '' ? Number(assignedToId) : null,
      });
      navigate(`/tickets/${res.data.id}`);
    } catch (err: unknown) {
      const apiErrors = (err as { response?: { data?: { errors?: Record<string, string[]> } } })?.response?.data?.errors;
      if (apiErrors) {
        setErrors({
          title:       apiErrors['Title']?.[0],
          description: apiErrors['Description']?.[0],
          priority:    apiErrors['Priority']?.[0],
        });
      } else {
        setSubmitError('Failed to create ticket. Please try again.');
      }
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="max-w-2xl mx-auto px-4 py-8">
      <Link to="/" className="inline-flex items-center text-sm text-blue-600 hover:underline mb-6">
        ← Back to tickets
      </Link>

      <div className="bg-white border border-gray-200 rounded-xl p-6">
        <h1 className="text-xl font-bold text-gray-900 mb-6">Create New Ticket</h1>

        <form onSubmit={handleSubmit} className="space-y-5" noValidate>
          {/* Title */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Title <span className="text-red-500">*</span>
            </label>
            <input
              type="text"
              value={title}
              onChange={e => setTitle(e.target.value)}
              placeholder="Brief summary of the issue"
              maxLength={200}
              className={`w-full px-3 py-2 border rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 ${errors.title ? 'border-red-400' : 'border-gray-300'}`}
            />
            {errors.title && <p className="text-red-600 text-xs mt-1">{errors.title}</p>}
          </div>

          {/* Description */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Description <span className="text-red-500">*</span>
            </label>
            <textarea
              value={description}
              onChange={e => setDescription(e.target.value)}
              placeholder="Describe the issue in detail…"
              rows={4}
              maxLength={2000}
              className={`w-full px-3 py-2 border rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 resize-none ${errors.description ? 'border-red-400' : 'border-gray-300'}`}
            />
            {errors.description && <p className="text-red-600 text-xs mt-1">{errors.description}</p>}
          </div>

          {/* Priority + Creator row */}
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Priority</label>
              <select
                value={priority}
                onChange={e => setPriority(e.target.value as TicketPriority)}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                {PRIORITIES.map(p => <option key={p} value={p}>{p}</option>)}
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Created by <span className="text-red-500">*</span>
              </label>
              {loadingUsers ? (
                <div className="h-9 bg-gray-100 rounded-lg animate-pulse" />
              ) : users.length > 0 ? (
                <select
                  value={createdById}
                  onChange={e => setCreatedById(Number(e.target.value))}
                  className={`w-full px-3 py-2 border rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 ${errors.createdById ? 'border-red-400' : 'border-gray-300'}`}
                >
                  {users.map(u => <option key={u.id} value={u.id}>{u.name} ({u.role})</option>)}
                </select>
              ) : (
                <input
                  type="number"
                  value={createdById || ''}
                  onChange={e => setCreatedById(Number(e.target.value))}
                  placeholder="User ID"
                  min={1}
                  className={`w-full px-3 py-2 border rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 ${errors.createdById ? 'border-red-400' : 'border-gray-300'}`}
                />
              )}
              {errors.createdById && <p className="text-red-600 text-xs mt-1">{errors.createdById}</p>}
            </div>
          </div>

          {/* Assign to */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Assign to <span className="text-gray-400 font-normal">(optional)</span></label>
            {users.length > 0 ? (
              <select
                value={assignedToId}
                onChange={e => setAssignedToId(e.target.value === '' ? '' : Number(e.target.value))}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                <option value="">Unassigned</option>
                {users.map(u => <option key={u.id} value={u.id}>{u.name} ({u.role})</option>)}
              </select>
            ) : (
              <input
                type="number"
                value={assignedToId}
                onChange={e => setAssignedToId(e.target.value === '' ? '' : Number(e.target.value))}
                placeholder="User ID (leave empty for unassigned)"
                min={1}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
              />
            )}
          </div>

          {submitError && (
            <div className="p-3 bg-red-50 border border-red-200 rounded-lg text-red-700 text-sm">
              {submitError}
            </div>
          )}

          <div className="flex items-center justify-end gap-3 pt-2">
            <Link to="/" className="px-4 py-2 text-sm text-gray-600 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors">
              Cancel
            </Link>
            <button
              type="submit"
              disabled={submitting}
              className="px-5 py-2 bg-blue-600 text-white text-sm font-medium rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
            >
              {submitting ? 'Creating…' : 'Create Ticket'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
