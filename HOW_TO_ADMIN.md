# Admin API

The application provides a small admin API for inspecting and modifying alerts and events.

All admin endpoints require an admin API key.

## 1. Configure admin API keys

Admin keys are configured through `appsettings.Development.json`.

Example:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AdminKeys": [
    "development-key-123",
    "another-development-key"
  ]
}
```

`appsettings.Development.json` is intentionally **not committed to Git**, as it contains secrets.

The file should exist locally in the project directory:

```text
YourProject/
├── appsettings.json
├── appsettings.Development.json    # local only
├── HOW_TO_ADMIN.md
└── ...
```

If you clone the repository, create `appsettings.Development.json` yourself and add your development keys.

## 2. Admin authentication

Every admin request must include the following HTTP header:

```text
X-Admin-Key: development-key-123
```

The key must match one of the keys configured under `AdminKeys`.

Example:

```text
X-Admin-Key: development-key-123
```

Requests without a valid key return:

```text
401 Unauthorized
```

## 3. Admin endpoints

The admin API is available under:

```text
/api/admin
```

### Alerts

#### Get all alerts

```http
GET /api/admin/alerts
X-Admin-Key: development-key-123
```

#### Get an alert

```http
GET /api/admin/alerts/{id}
X-Admin-Key: development-key-123
```

#### Create an alert

```http
POST /api/admin/alerts
Content-Type: application/json
X-Admin-Key: development-key-123
```

Example body:

```json
{
  "name": "BTC 5% movement",
  "enabled": true,
  "symbol": "BTC",
  "thresholdPercentage": 5,
  "channels": [
    "email",
    "slack"
  ]
}
```

#### Update an alert

```http
PUT /api/admin/alerts/{id}
Content-Type: application/json
X-Admin-Key: development-key-123
```

Use the alert ID returned when creating or retrieving the alert.

#### Delete an alert

```http
DELETE /api/admin/alerts/{id}
X-Admin-Key: development-key-123
```

### Events

#### Get all events

```http
GET /api/admin/events
X-Admin-Key: development-key-123
```

#### Get an event

```http
GET /api/admin/events/{id}
X-Admin-Key: development-key-123
```

#### Delete an event

```http
DELETE /api/admin/events/{id}
X-Admin-Key: development-key-123
```

## 4. Testing with Postman

For example, to retrieve all alerts:

```text
GET http://localhost:5034/api/admin/alerts
```

In the **Headers** section add:

| Key | Value |
|---|---|
| `X-Admin-Key` | `development-key-123` |

For POST and PUT requests, also set:

```text
Content-Type: application/json
```

## 5. Creating and testing an alert

A simple end-to-end test can be performed as follows.

### Step 1 — Create an alert

```http
POST /api/admin/alerts
```

Body:

```json
{
  "name": "BTC 5% movement",
  "enabled": true,
  "symbol": "BTC",
  "thresholdPercentage": 5,
  "channels": [
    "email",
    "slack"
  ]
}
```

Save the returned alert ID.

### Step 2 — Send a test event

The normal event ingestion endpoint does not require admin access:

```http
POST /api/events
Content-Type: application/json
```

Body:

```json
{
  "type": "market.movement",
  "data": {
    "symbol": "BTC",
    "changePercentage": 7.2,
    "occurredAt": "2026-09-08T18:30:00Z"
  }
}
```

The 7.2% movement exceeds the configured 5% threshold.

The alert engine creates notifications for both configured channels:

```text
BTC movement event
        ↓
MarketMovementAlert matches
        ↓
┌───────────────────┐
│ Email Notification│
└───────────────────┘
        +
┌───────────────────┐
│ Slack Notification│
└───────────────────┘
        ↓
Message Queue
        ↓
Notification Worker
        ↓
Notification Dispatcher
```

The current Email and Slack implementations are placeholders and write the notification to the application console rather than sending real messages.

## 6. Security note

Do **not** commit real API keys or other secrets to Git.

`appsettings.Development.json` is included in `.gitignore` for this reason.

For a production deployment, secrets should be supplied through a proper secret-management mechanism or environment variables rather than storing them in configuration files.