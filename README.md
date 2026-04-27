# micro-health
What is this? With absolute honesty: just an upskilling playground. I asked an LLM to generate a project idea with certain characteristics that I was interested in becoming proficient with and I started hacking away.
The output was basically a health tracking full-stack application, tailored towards sports. But I don't have a real direction, as this isn't a real product. Meaning that my roadmap is subject to constant changes.

The main idea was to grow more comfortable dealing with infrastructure and containerization, while also acquiring new backend skills and reviewing old ones. 

Main topics to learn:

- Microservices architecture with fully async communication powered by RabbitMQ
- Docker & Docker Compose
- Polyglot system comprised of:
  - Kotlin Spring Boot 4
  - C# ASP.NET Core 10
  - TypeScript Fastify with Nest.js
  - Python FastAPI
- TimeScale DB, just because
- Centralized authentication and authorization using Keycloak
- Apache APISIX gateway
- Contract-based testing
- Observability

For the rest, I'm just making it up as I move forward.

## How to run
This is still far from being complete, but a simple

```sh
docker compose up -d
```

would do the job.