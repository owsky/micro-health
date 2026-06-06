# --- STAGE 1: Build ---
FROM node:24.16.0-alpine3.23 AS builder

WORKDIR /usr/src/app

# install pnpm
RUN npm install -g pnpm@latest-11

# copy the package and package lock
COPY package.json pnpm-lock.yaml pnpm-workspace.yaml ./

# install dependencies
RUN pnpm ci

# copy the rest of the application source
COPY . .

# build the NestJS application
RUN pnpm run build

# prune development dependencies to keep the production image small
RUN pnpm prune --prod

# --- STAGE 2: Production Run ---
FROM node:24.16.0-alpine3.23 AS runner

WORKDIR /usr/src/app

# set Node environment to production
ENV NODE_ENV=production

# copy the pruned node_modules and built dist folder from builder
COPY --from=builder /usr/src/app/node_modules ./node_modules
COPY --from=builder /usr/src/app/dist ./dist
COPY --from=builder /usr/src/app/package.json ./package.json

# start the application using the compiled main.js file
CMD ["node", "dist/src/main"]