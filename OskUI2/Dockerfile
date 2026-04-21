# 1. Aşama: Build aşaması (Node.js ile)
FROM node:20-alpine AS build-stage
WORKDIR /app

# Bağımlılıkları yükle
COPY package*.json ./
RUN npm install

# Kaynak kodları kopyala ve build al
COPY . .
RUN npm run build -- --configuration=production

# 2. Aşama: Yayın aşaması (Nginx ile)
FROM nginx:stable-alpine

# Angular routing hatalarını (404) önlemek için nginx ayarını doğrudan içine yazıyoruz
RUN echo 'server { \
    listen 80; \
    location / { \
        root /usr/share/nginx/html; \
        index index.html index.htm; \
        try_files $uri $uri/ /index.html; \
    } \
}' > /etc/nginx/conf.d/default.conf

# Build aşamasından gelen dosyaları kopyala
# Senin paylaştığın loglara göre yol: /app/dist/OskUI/browser
COPY --from=build-stage /app/dist/OskUI/browser /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]