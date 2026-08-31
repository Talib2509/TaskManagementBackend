# TaskManagement Backend API

Qısa frontend reference. Swagger: `/swagger` (Development).

## Base URL və auth

- Docker: `http://localhost:5000`
- Local HTTPS: `https://localhost:7120`
- JWT tələb edən endpoint-lər üçün header:

```http
Authorization: Bearer <accessToken>
```

Ümumi qayda: uğurlu cavab `200 OK`, validation/business xətası əsasən `400`, auth xətası `401`, tapılmayan resurs `404` qaytarır.

## Auth

| Method | Endpoint | Auth | Body / Query |
|---|---|---|---|
| `POST` | `/api/Auth/register` | Xeyr | `multipart/form-data`: `FullName`, `Email`, `Password`, `CompanyName` |
| `POST` | `/api/Auth/login` | Xeyr | `multipart/form-data`: `Email`, `Password` |
| `POST` | `/api/Auth/refresh-token-login` | Xeyr | `multipart/form-data`: `RefreshToken` |
| `GET` | `/api/Auth/confirm-email?userId=<guid>&token=<token>` | Xeyr | Query parametrləri |
| `POST` | `/api/Auth/forgot-password` | Xeyr | `multipart/form-data`: request DTO sahələri |
| `POST` | `/api/Auth/reset-password` | Xeyr | `multipart/form-data`: request DTO sahələri |

`login` uğurlu olduqda response içində `token`, `refreshToken`, `expireDate` qaytarılır.

## Tasks

Bütün endpoint-lər JWT tələb edir. `id` dəyərləri `Guid`-dir.

| Method | Endpoint | Body / Query |
|---|---|---|
| `GET` | `/api/tasks` | Siyahı/filter query parametrləri |
| `GET` | `/api/tasks/{id}` | — |
| `POST` | `/api/tasks` | JSON: `Title`, `Description`, `Priority`, `Visibility`, `Deadline` |
| `PUT` | `/api/tasks/{id}` | JSON: update DTO |
| `DELETE` | `/api/tasks/{id}` | — |
| `PATCH` | `/api/tasks/{id}/status` | JSON: `{ "newStatus": ... }` |
| `GET` | `/api/tasks/board` | — |
| `GET` | `/api/tasks/search?q=<text>` | `q` |
| `POST` | `/api/tasks/team` | JSON: team-task DTO |
| `POST` | `/api/tasks/{id}/claim` | — |
| `POST` | `/api/tasks/reassign` | JSON: reassign DTO |
| `POST` | `/api/tasks/{taskId}/subtasks` | JSON: `{ "text": "..." }` |
| `PATCH` | `/api/tasks/subtasks/{subTaskId}/toggle` | — |
| `GET` | `/api/tasks/my-team-tasks` | — |
| `GET` | `/api/tasks/team-dashboard?teamId=<guid>` | `teamId` |

## Task items

| Method | Endpoint | Body / Query |
|---|---|---|
| `GET` | `/api/TaskItem` | — |
| `GET` | `/api/TaskItem/{id}` | — |
| `GET` | `/api/TaskItem/team/{teamId}` | — |
| `GET` | `/api/TaskItem/my-tasks?userId=<guid>` | `userId` |
| `POST` | `/api/TaskItem` | JSON: `Title`, `Description`, `TeamId`, `AssignedUserId`, `DueDate`, `Priority` |
| `PUT` | `/api/TaskItem` | JSON: update DTO |
| `PUT` | `/api/TaskItem/change-status` | JSON: status DTO |
| `DELETE` | `/api/TaskItem/{id}` | — |

## Teams, members və invitations

| Method | Endpoint | Auth / Body |
|---|---|---|
| `GET` | `/api/Team` | JWT |
| `GET` | `/api/Team/{id}` | JWT |
| `GET` | `/api/Team/my-teams?userId=<guid>` | JWT |
| `GET` | `/api/Team/{id}/statistics` | JWT |
| `POST` | `/api/Team` | JWT + JSON team DTO |
| `PUT` | `/api/Team` | JWT + JSON update DTO |
| `PUT` | `/api/Team/assign-lead` | JWT + JSON request DTO |
| `DELETE` | `/api/Team/{id}` | JWT |
| `GET` | `/api/TeamMember` | JWT |
| `GET` | `/api/TeamMember/{id}` | JWT |
| `POST` | `/api/TeamMember` | JWT + JSON member DTO |
| `GET` | `/api/TeamInvitation` | JWT |
| `GET` | `/api/TeamInvitation/{id}` | JWT |
| `POST` | `/api/TeamInvitation` | JWT + JSON invitation DTO |
| `PUT` | `/api/TeamInvitation/accept` | JWT + JSON request DTO |
| `PUT` | `/api/TeamInvitation/reject` | JWT + JSON request DTO |
| `DELETE` | `/api/TeamInvitation/{id}` | JWT |

## Company və profile

| Method | Endpoint | Auth / Body |
|---|---|---|
| `GET` | `/api/Company` | JWT |
| `GET` | `/api/Company/{id}` | JWT |
| `GET` | `/api/Company/owner/{ownerId}` | JWT |
| `GET` | `/api/Company/my-company` | JWT |
| `GET` | `/api/Company/{id}/statistics` | JWT |
| `POST` | `/api/Company` | JWT + JSON company DTO |
| `PUT` | `/api/Company` | JWT + JSON update DTO |
| `DELETE` | `/api/Company/{id}` | JWT |
| `GET` | `/api/Profile/me` | JWT |
| `PUT` | `/api/Profile/me` | JWT + JSON profile DTO |
| `POST` | `/api/Profile/upload-avatar` | JWT + `multipart/form-data`: `file` |
| `POST` | `/api/Profile/change-password` | JWT + JSON password DTO |
| `POST` | `/api/Profile/deactivate` | JWT |
| `GET` | `/api/Profile/settings` | JWT |
| `PUT` | `/api/Profile/settings` | JWT + JSON settings DTO |

## Attachments, comments və notifications

| Method | Endpoint | Auth / Body |
|---|---|---|
| `POST` | `/api/Attachments/task/{taskId}` | JWT + multipart: `file` |
| `POST` | `/api/Attachments/task/{taskId}/chunk` | JWT + multipart: `chunk`, `fileGuid`, `fileName`, `chunkIndex`, `totalChunks` |
| `GET` | `/api/Attachments/task/{taskId}` | JWT |
| `GET` | `/api/Attachments/download/{id}` | JWT; binary file response |
| `DELETE` | `/api/Attachments/{id}` | JWT |
| `GET` | `/api/Comments/task/{taskId}` | Auth controller-də məcburi deyil |
| `GET` | `/api/Comments/{taskId}/timeline` | Auth controller-də məcburi deyil |
| `POST` | `/api/Comments` | multipart comment DTO |
| `PUT` | `/api/Comments` | JWT + multipart comment DTO |
| `DELETE` | `/api/Comments/{id}` | multipart tələb etmir |
| `POST` | `/api/Comments/reactions` | multipart reaction DTO |
| `GET` | `/api/Notification/{id}?userId=<guid>` | JWT |
| `GET` | `/api/Notification/my-notifications?userId=<guid>&onlyUnread=false&page=1&pageSize=10` | JWT |
| `GET` | `/api/Notification/unread?userId=<guid>` | JWT |
| `GET` | `/api/Notification/count?userId=<guid>&onlyUnread=true` | JWT |
| `POST` | `/api/Notification` | JWT + JSON notification DTO |
| `POST` | `/api/Notification/send` | JWT + JSON request DTO |
| `PUT` | `/api/Notification/mark-as-read` | JWT + JSON request DTO |
| `PUT` | `/api/Notification/mark-all-as-read` | JWT + JSON request DTO |
| `DELETE` | `/api/Notification/{id}?userId=<guid>` | JWT |
| `GET` | `/api/Notifications?type=<type>` | JWT |
| `POST` | `/api/Notifications/mark-as-read` | JWT |

## Roles və reports

Roles endpoint-ləri yalnız `Admin` və `SuperAdmin` üçündür.

| Method | Endpoint | Auth / Body |
|---|---|---|
| `GET` | `/api/Roles` və `/api/Roles/all` | Admin/SuperAdmin |
| `GET` | `/api/Roles/{id}` | Admin/SuperAdmin |
| `POST` | `/api/Roles` | Admin/SuperAdmin + JSON role DTO |
| `PUT` | `/api/Roles/{id}` | Admin/SuperAdmin + JSON role DTO |
| `DELETE` | `/api/Roles/{id}` | Admin/SuperAdmin |
| `POST` | `/api/Roles/users/{userId}/assign` | Admin/SuperAdmin + JSON role DTO |
| `DELETE` | `/api/Roles/users/{userId}/{roleName}` | Admin/SuperAdmin |
| `GET` | `/api/Roles/users/{userId}` | Admin/SuperAdmin |
| `GET` | `/api/Roles/{roleId}/users` | Admin/SuperAdmin |
| `GET` | `/api/admin/dashboard/stats?days=30` | Admin/SuperAdmin |
| `GET` | `/api/company-dashboard/stats` | JWT |
| `GET` | `/api/company-dashboard/{companyId}/stats` | JWT |
| `GET` | `/api/audit-logs` | Admin/SuperAdmin + filter query |
| `GET` | `/api/audit-logs/{id}` | Admin/SuperAdmin |

## Reports

Bütün report endpoint-ləri JWT tələb edir. `fromDate` və `toDate` optional query parametrləridir.

- `GET /api/Reports/team/{teamId}/excel?fromDate=...&toDate=...`
- `GET /api/Reports/team/{teamId}/pdf?fromDate=...&toDate=...`
- `GET /api/Reports/user/{userId}/excel?fromDate=...&toDate=...`
- `GET /api/Reports/user/{userId}/pdf?fromDate=...&toDate=...`
- `GET /api/Reports/me/excel?fromDate=...&toDate=...`
- `GET /api/Reports/me/pdf?fromDate=...&toDate=...`
- `GET /api/Reports/company/{companyId}/excel?fromDate=...&toDate=...`
- `GET /api/Reports/company/{companyId}/pdf?fromDate=...&toDate=...`

Excel/PDF endpoint-ləri binary fayl qaytarır. Frontend-də `response.blob()` istifadə edin.

## Fetch nümunələri

```javascript
const API = "http://localhost:5000";

async function apiFetch(path, options = {}) {
  const token = localStorage.getItem("accessToken");
  const headers = new Headers(options.headers || {});

  if (token) headers.set("Authorization", `Bearer ${token}`);
  if (!(options.body instanceof FormData)) headers.set("Content-Type", "application/json");

  const response = await fetch(`${API}${path}`, { ...options, headers });
  const data = response.headers.get("content-type")?.includes("json")
	? await response.json()
	: await response.blob();

  if (!response.ok) throw new Error(data?.message || `HTTP ${response.status}`);
  return data;
}

// Login: endpoint multipart/form-data tələb edir.
const form = new FormData();
form.append("Email", email);
form.append("Password", password);
const login = await apiFetch("/api/Auth/login", { method: "POST", body: form });
localStorage.setItem("accessToken", login.token);

// JSON request
const task = await apiFetch("/api/tasks", {
  method: "POST",
  body: JSON.stringify({
	title: "Yeni task",
	description: "Task açıqlaması",
	priority: 1,
	visibility: 0,
	deadline: null
  })
});
```

Qeyd: `FormData` göndərərkən `Content-Type` header-ini əl ilə yazmayın; browser boundary dəyərini özü əlavə edir. Dəqiq DTO sahələri və response modelləri üçün Swagger istifadə edilə bilər.
