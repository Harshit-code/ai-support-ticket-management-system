# AI Support Ticket Management System

A full-stack support ticket management system built with **ASP.NET Core 8 Web API** (backend) and **React + TypeScript** (frontend).

---

## Prerequisites

Make sure the following are installed before you start:

| Tool | Version | Install |
|---|---|---|
| .NET SDK | 8.0+ | [dotnet.microsoft.com/download](https://dotnet.microsoft.com/download) |
| Node.js + npm | 18+ | [nodejs.org](https://nodejs.org) |
| EF Core CLI | any | `dotnet tool install --global dotnet-ef` |

> **Windows PowerShell users:** if `npm` is blocked, run this once and restart your terminal:
> ```powershell
> Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
> ```

---

## Clone the repository

```bash
git clone https://github.com/Harshit-code/ai-support-ticket-management-system.git
cd ai-support-ticket-management-system
```

---

## Run the backend

Open a terminal and run:

```bash
cd src/SupportTickets.Api
dotnet run
```

The backend will start at **http://localhost:5020**

- The SQLite database (`support_tickets.db`) is created automatically on first startup — no manual migration needed.
- Seed data (4 users, 3 tickets, 2 comments) is applied automatically.
- Swagger UI for testing the API: **http://localhost:5020/swagger**

---

## Run the frontend

Open a **second terminal** (keep the backend running) and run:

```bash
cd frontend
npm install
npm run dev
```

The frontend will start at **http://localhost:5173**

> `npm install` only needs to be run once after cloning. After that, just `npm run dev`.

---

## Run the tests

```bash
cd tests/SupportTickets.Api.Tests
dotnet test
```

62 unit tests covering status transition logic (state machine + controller layer).

---

## Project structure

```
ai-support-ticket-management-system/
│
├── src/                          # .NET solution
│   ├── SupportTickets.Api/       # ASP.NET Core Web API (controllers, DTOs, Program.cs)
│   ├── SupportTickets.Domain/    # Entities, enums, interfaces, services, state machine
│   └── SupportTickets.Infrastructure/  # EF Core, DbContext, repositories, migrations
│
├── frontend/                     # React + TypeScript + Tailwind CSS
│   └── src/
│       ├── api/                  # Axios API clients (tickets, comments, users)
│       ├── components/           # Shared UI components
│       ├── pages/                # TicketListPage, TicketDetailPage, CreateTicketPage
│       ├── types/                # TypeScript types matching backend entities
│       └── utils/                # statusTransitions.ts (mirrors backend state machine)
│
├── tests/
│   └── SupportTickets.Api.Tests/ # xUnit + Moq unit tests
│
└── *.md                          # Project documentation
```

---

## Features

- **Ticket management** — create, view, update, and filter tickets by keyword or status
- **Status transitions** — enforced state machine: `Open → InProgress → Resolved → Closed`, cancellable from `Open` or `InProgress`
- **Comments** — add and view comments per ticket
- **Validation** — field validation (400), not found (404), invalid transition (409 with allowed transitions listed)
- **Seed data** — pre-loaded users, tickets, and comments on first run

---

## Tech stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core 8, EF Core 8, SQLite |
| Frontend | React 18, TypeScript, Vite, Tailwind CSS, Axios |
| Tests | xUnit, Moq |
