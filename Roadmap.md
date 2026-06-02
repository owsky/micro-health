# Architecture Overview

System built as a **polyglot microservice platform** using containers, contract-first APIs, and event-driven communication.

Core infrastructure components:

* API gateway: Apache APISIX
* Identity provider: Keycloak
* Message broker: RabbitMQ
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
     APISIX
       |
---------------------------------------------------------
|        |         |        |         |                 |
User   Workout   Metrics   Feed    Analytics       Notification
Svc     Svc       Svc      Svc      Svc               Svc
 |        |         |        |
Postgres Postgres Timescale MongoDB
           |
        RabbitMQ
           |
   Event-driven processing
```

Key architectural principles:

* **each service owns its database**
* **services communicate asynchronously via RabbitMQ events**
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
  apisix/
  keycloak/
  rabbitmq/
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

* APISIX
* Keycloak
* RabbitMQ
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
* service account clients
* roles
* JWT tokens

User roles example:

```
USER
ADMIN
```

### APISIX configuration

Features:

* declarative route configuration via `apisix.yaml`
* OpenID Connect plugin with Keycloak (bearer token validation)
* identity header injection (`X-Userinfo`, `X-Access-Token`) to upstream services
* rate limiting
* request ID generation for distributed tracing

Deliverable:

```
client → apisix → service
```

secured via Keycloak-issued JWT, validated by APISIX's OpenID Connect plugin.

---

# Phase 3 — User Service

Technology: Spring Boot (Kotlin)
Database: PostgreSQL

### Responsibilities

User identity and fitness profile management.

### Features

Profile management:

* create user profile (triggered on first authenticated request)
* retrieve own profile
* update profile
* delete account

Fitness profile:

* height
* weight
* date of birth
* fitness goals
* training experience level

User preferences:

* units (metric / imperial)
* notification preferences

Privacy:

* public vs private profile

### Endpoints

```
GET    /profiles/v1/me
GET    /profiles/v1/{username}
POST   /profiles/v1/me
PATCH  /profiles/v1/me
DELETE /profiles/v1/me
GET    /preferences/v1/me
PUT    /preferences/v1/me
GET    /fitness-goals/v1/me
PUT    /fitness-goals/v1/me
```

### Events Produced

```
UserCreated
UserUpdated
UserDeleted
PreferencesUpdated
FitnessGoalsUpdated
```

---

# Phase 4 — Workout Service

Technology: .NET Core
Database: SQL Server

### Responsibilities

Exercise catalog management and recorded workout storage.

### Features

Exercise catalog:

* create and list exercises
* categorize by muscle group and difficulty level
* search and filter exercises

Workout templates:

* create workout plans from exercises
* define sets, reps, and rest times per exercise
* list and manage own templates

Recorded workouts:

* create a recorded workout from a template
* store performed sets with actual reps, weight, and notes

User data lifecycle:

* purge all exercises, templates, and recorded workouts for a user on account deletion

### Endpoints

```
GET    /exercise-catalog
GET    /exercise-catalog/{id}
POST   /exercise-catalog
PUT    /exercise-catalog/{id}
DELETE /exercise-catalog/{id}
GET    /workout-templates
GET    /workout-templates/{id}
POST   /workout-templates
PUT    /workout-templates/{id}
DELETE /workout-templates/{id}
GET    /workouts
GET    /workouts/{id}
POST   /workouts
PUT    /workouts/{id}
DELETE /workouts/{id}

```

### Events Consumed

```
UserDeleted
```

---

# Phase 5 — Metrics Service

Technology: Fastify
Database: TimescaleDB

### Responsibilities

Store and query high-frequency physiological metrics.

### Features

Metric ingestion:

* heart rate
* steps
* calories burned
* sleep duration

Time-range queries:

* query by metric type and time range
* daily and weekly aggregations (min, max, avg)

Data retention policy (auto-drop old raw data).

Data purge:

* delete all metrics for a user on account deletion

### Endpoints

```
POST /metrics
GET  /metrics?type=heartRate&from=&to=
GET  /metrics/summary/daily
GET  /metrics/summary/weekly
```

### Events Consumed

```
UserDeleted
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

* workout activity posts (created via event from Workout Service)
* achievement posts (created via event from Analytics Service)

User data lifecycle:

* sync display name and avatar in posts on profile update
* purge all posts, likes, and comments on account deletion

User interactions:

* like / unlike an activity
* add and delete comments

Feed retrieval:

* personal feed (activities from followed users)
* profile activity feed for any user
* cursor-based pagination

### Endpoints

```
GET    /feed
GET    /feed/users/{id}
POST   /feed/{id}/like
DELETE /feed/{id}/like
POST   /feed/{id}/comments
DELETE /feed/comments/{commentId}
```

### Events Consumed

```
WorkoutCompleted
AchievementUnlocked
UserUpdated
UserDeleted
```

### Events Produced

```
FeedCommentAdded
FeedLiked
```

---

# Phase 7 — Analytics Service

Technology: Spring Boot (Kotlin)

### Responsibilities

Fitness analysis and training insights derived from workout and metrics data.

### Features

Workout analytics:

* total training volume over a period
* average workout duration
* workout frequency per week

Health analytics:

* heart rate trends
* calorie trends

Performance insights:

* weekly and monthly progress summaries
* training intensity score

Rule-based recommendations:

* suggested rest days based on recent workout frequency
* suggested volume adjustments based on trend

User data lifecycle:

* purge all analytics data for a user on account deletion

### Endpoints

```
GET /analytics/weekly
GET /analytics/monthly
GET /analytics/progress
GET /analytics/recovery
```

### Events Consumed

```
WorkoutCompleted
MetricRecorded
UserDeleted
```

### Events Produced

```
AchievementUnlocked
```

---

# Phase 8 — Notification Service

Technology: Fastify

### Responsibilities

In-app notification delivery for fitness and social events.

### Features

Notification creation (via consumed events):

* welcome notification on account creation
* workout completion confirmation
* achievement unlocked
* new comment on own activity
* new like on own activity

User data lifecycle:

* sync notification preferences on profile update
* purge all notifications for a user on account deletion

Notification storage and retrieval:

* list notifications (unread first)
* mark individual notification as read
* mark all as read

Notification filtering by user preferences (set in User Service).

### Endpoints

```
GET  /notifications
POST /notifications/{id}/read
POST /notifications/read-all
```

### Events Consumed

```
WorkoutCompleted
AchievementUnlocked
FeedCommentAdded
FeedLiked
UserCreated
UserUpdated
UserDeleted
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

Workout recording interface:

* create a workout record from a template or manual entry
* enter performed sets/reps/weight
* save draft workouts and finalize completed workouts

Workout history.

Pages:

```
exercises
create workout
record workout
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
* RabbitMQ message processing

Deliverable:

full system observability.

---

# Phase 14 — Testing

Testing layers:

Unit tests for each service.

Integration tests:

* database interactions
* RabbitMQ messaging

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
