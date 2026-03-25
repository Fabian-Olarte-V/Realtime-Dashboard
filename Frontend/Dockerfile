#Stage 1: Install dependencies
FROM node:22-alpine3.23 AS deps
WORKDIR /app
COPY package*.json ./

RUN npm ci


#Stage 2: Build
FROM node:22-alpine3.23 AS builder
WORKDIR /app
COPY --from=deps /app/node_modules ./node_modules
COPY . .

RUN npm run build


#Stage 3: Angular APP Image
FROM nginx:alpine
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=builder /app/dist/frontend/browser /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]