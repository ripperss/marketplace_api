# Копирование файлов и сборка
FROM node:lts-alpine AS build
WORKDIR /front
RUN chown -R node:node /front
USER node

COPY . .
RUN npm install

# Запуск
FROM build AS run
EXPOSE 3000
CMD ["npm", "run", "dev"]