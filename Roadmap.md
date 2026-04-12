# Architecture Overview

System built as a **polyglot microservice platform** using containers, contract-first APIs, and event-driven communication.

Core infrastructure components:

* API gateway: Kong
* Identity provider: Keycloak
* Event streaming: Apache Kafka
* Monitoring: Prometheus
* Dashboards: Grafana
* Distributed tracing: OpenTelemetry
* Relational DB: PostgreSQL
* Time-series DB: TimescaleDB
* Document DB: MongoDB

API contracts are defined using OpenAPI Specification and clients generated via OpenAPI Generator.

---

# Final System Architecture

```
React Microfrontends
       |
       Kong
       |
---------------------------------------------------------
|        |         |        |         |                 |
User   Workout   Metrics   Feed    Analytics       Notification
Svc     Svc       Svc      Svc      Svc               Svc
 |        |         |        |
Postgres Postgres Timescale MongoDB
           |
          Kafka
           |
   Event-driven processing
```

Key architectural principles:

* **each service owns its database**
* **services communicate asynchronously via events**
* **frontend uses generated API clients**
* **observability integrated everywhere**

---

# Monorepo Structure

```
fitness-platform/

apps/
  dashboard-frontend
  workouts-frontend
  analytics-frontend
  social-frontend

services/
  user-service
  workout-service
  metrics-service
  feed-service
  analytics-service
  notification-service

contracts/
  openapi/
  events/

infrastructure/
  kong/
  keycloak/
  kafka/
  observability/

docker-compose.yml
```

---

# Roadmap

Each phase keeps the system runnable.

---

# Phase 1 — Repository and Infrastructure

Goal: working container environment.

### Tasks

Create repository structure.

Implement root **docker-compose** including:

* Kong
* Keycloak
* Kafka
* PostgreSQL instances
* MongoDB
* TimescaleDB
* Prometheus
* Grafana
* OpenTelemetry collector

Configure networking and health checks.

### Deliverables

Running platform infrastructure.

```
docker compose up
```

---

# Phase 2 — Authentication and Gateway

Goal: secure entrypoint.

### Keycloak configuration

Features:

* authentication realm
* frontend clients
* service clients
* roles
* JWT tokens

User roles example:

```
USER
ADMIN
```

### Kong configuration

Features:

* routing
* JWT validation
* rate limiting
* logging
* upstream service registration

Deliverable:

```
client → kong → service
```

secured via JWT.

---

# Phase 3 — User Service

Technology: Spring Boot
Database: PostgreSQL

### Responsibilities

User identity and fitness profile management.

### Features

Account management:

* create user profile
* update profile
* retrieve profile
* deactivate account

Fitness profile:

* height
* weight
* age
* fitness goals
* training experience level

User preferences:

* notification preferences
* visibility settings
* units (metric/imperial)

Privacy:

* public vs private profile
* social visibility settings

### Endpoints

```
GET /users/me
PUT /users/me
GET /users/{id}
PUT /users/preferences
DELETE /users/me
```

### Events Produced

```
UserCreated
UserProfileUpdated
UserDeleted
```

---

# Phase 4 — Workout Service

Technology: .NET Core
Database: PostgreSQL

### Responsibilities

Workout planning and workout execution tracking.

### Features

Exercise catalog:

* create exercises
* categorize by muscle group
* difficulty level
* equipment requirements

Workout plan builder:

* create workout templates
* add exercises
* define sets/reps
* define rest times

Workout execution:

* start workout session
* record sets
* record repetitions
* record weights

Workout completion:

* finalize workout
* calculate duration
* calculate volume

History:

* list completed workouts
* retrieve workout details
* workout statistics

### Endpoints

```
GET /exercises
POST /exercises
POST /workouts
GET /workouts
POST /workouts/{id}/start
POST /workouts/{id}/complete
GET /workouts/history
```

### Events Produced

```
WorkoutStarted
WorkoutCompleted
```

---

# Phase 5 — Metrics Service

Technology: Fastify
Database: TimescaleDB

### Responsibilities

Store high-frequency physiological metrics.

### Features

Metric ingestion:

* heart rate
* steps
* calories burned
* sleep metrics

High throughput ingestion API.

Aggregations:

* daily metrics
* weekly metrics
* moving averages

Metric queries:

* query by time range
* query by metric type

Data retention policies.

### Endpoints

```
POST /metrics
GET /metrics?type=heartRate
GET /metrics/summary/daily
GET /metrics/summary/weekly
```

### Events Produced

```
MetricRecorded
```

---

# Phase 6 — Feed Service

Technology: Fastify
Database: MongoDB

### Responsibilities

Activity feed and social interactions.

### Features

Feed generation:

* workout activity posts
* achievement posts
* social updates

User interactions:

* like activity
* comment activity
* delete comment

Feed retrieval:

* user feed
* profile activity feed

Pagination support.

### Endpoints

```
GET /feed
GET /feed/user/{id}
POST /feed/{id}/like
POST /feed/{id}/comment
DELETE /feed/comment/{id}
```

### Events Consumed

```
WorkoutCompleted
AchievementUnlocked
```

### Events Produced

```
FeedCommentAdded
FeedLiked
```

---

# Phase 7 — Analytics Service

Technology: Spring Boot

### Responsibilities

Fitness analysis and training insights.

### Features

Workout analytics:

* total training volume
* average workout duration
* frequency analysis

Health analytics:

* heart rate trends
* calorie trends
* recovery metrics

Performance insights:

* weekly progress
* monthly trends
* training intensity score

Recommendations:

* suggested rest days
* suggested training volume

### Endpoints

```
GET /analytics/weekly
GET /analytics/progress
GET /analytics/recovery
GET /analytics/performance
```

### Events Consumed

```
WorkoutCompleted
MetricRecorded
```

### Events Produced

```
AchievementUnlocked
TrainingInsightGenerated
```

---

# Phase 8 — Notification Service

Technology: Fastify

### Responsibilities

User notification delivery.

### Features

Notification generation:

* workout completion
* achievements
* social interactions

Notification storage.

Notification delivery:

* in-app notifications
* optional email support

User notification settings.

### Endpoints

```
GET /notifications
POST /notifications/read
POST /notifications/read-all
```

### Events Consumed

```
WorkoutCompleted
AchievementUnlocked
FeedCommentAdded
FeedLiked
```

---

# Phase 9 — Dashboard Frontend

React microfrontend.

### Features

Dashboard summary:

* weekly workout summary
* latest metrics
* quick analytics overview

Activity overview:

* recent workouts
* recent notifications

User profile management.

Pages:

```
dashboard
profile
notifications
```

---

# Phase 10 — Workouts Frontend

React microfrontend.

### Features

Exercise catalog browsing.

Workout builder:

* create workout plans
* edit workout plans

Workout execution interface:

* active workout session UI
* record sets/reps
* complete workout

Workout history.

Pages:

```
exercises
create workout
active workout
history
```

---

# Phase 11 — Analytics Frontend

React microfrontend.

### Features

Charts and analytics dashboards.

Metrics visualization:

* heart rate trends
* calorie trends

Workout analytics:

* training volume
* intensity metrics

Performance insights.

Pages:

```
analytics dashboard
performance
recovery insights
```

---

# Phase 12 — Social Frontend

React microfrontend.

### Features

Activity feed.

User interactions:

* like activities
* comment activities

Profile pages.

Leaderboards and challenges (optional).

Pages:

```
activity feed
user profile
leaderboard
```

---

# Phase 13 — Observability

Integrate monitoring.

### Metrics

Prometheus scrapes service metrics.

### Dashboards

Grafana dashboards:

* service latency
* request volume
* error rates

### Tracing

OpenTelemetry tracing for:

* gateway requests
* service-to-service calls
* Kafka processing

Deliverable:

full system observability.

---

# Phase 14 — Testing

Testing layers:

Unit tests for each service.

Integration tests:

* database interactions
* Kafka messaging

Contract tests:

OpenAPI validation.

End-to-end tests:

```
login
create workout
record metrics
view analytics
see feed updates
```

---

# Final Result

A complete microservice system demonstrating:

* polyglot architecture
* event-driven communication
* contract-first APIs
* distributed observability
* container orchestration with docker compose

This project simulates a **production-grade backend platform**, providing hands-on experience with modern distributed system architecture.
