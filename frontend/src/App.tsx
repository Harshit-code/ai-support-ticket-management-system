import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import TicketListPage from './pages/TicketListPage';
import TicketDetailPage from './pages/TicketDetailPage';
import CreateTicketPage from './pages/CreateTicketPage';

export default function App() {
  return (
    <BrowserRouter>
      <div className="min-h-screen bg-gray-50">
        {/* Nav */}
        <header className="bg-white border-b border-gray-200 sticky top-0 z-10">
          <div className="max-w-5xl mx-auto px-4 h-14 flex items-center">
            <a href="/" className="text-base font-semibold text-gray-900 hover:text-blue-600 transition-colors">
              🎫 Support Tickets
            </a>
          </div>
        </header>

        {/* Pages */}
        <main>
          <Routes>
            <Route path="/"              element={<TicketListPage />}   />
            <Route path="/tickets/new"   element={<CreateTicketPage />} />
            <Route path="/tickets/:id"   element={<TicketDetailPage />} />
            <Route path="*"              element={<Navigate to="/" replace />} />
          </Routes>
        </main>
      </div>
    </BrowserRouter>
  );
}
