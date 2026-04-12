# Stage 1: Build stage
FROM maven:3.9.14-eclipse-temurin-25-alpine AS build
WORKDIR /app

# Step 1: Cache dependencies (this only runs if pom.xml changes)
COPY pom.xml .
# RUN mvn dependency:go-offline
RUN --mount=type=cache,target=/root/.m2 \
    mvn -B dependency:go-offline

# Step 2: Build the jar
COPY src ./src
# RUN mvn clean package -DskipTests
RUN --mount=type=cache,target=/root/.m2 \
    mvn -B clean package -DskipTests

# Stage 2: Runtime stage
FROM eclipse-temurin:25-jre-alpine
WORKDIR /app

# Install curl for health checks and debugging
RUN apk add --no-cache curl

# Expose the port
EXPOSE 5000

# Copy the built jar from the build stage
COPY --from=build /app/target/*.jar app.jar

# Explicitly use a non-root user for better security (optional but recommended)
RUN addgroup -S spring && adduser -S spring -G spring
USER spring:spring

ENTRYPOINT ["java", "-jar", "app.jar"]