FROM node:12.18.4-alpine AS build
RUN npm install -g @angular/cli
RUN mkdir /client
WORKDIR /client
COPY . .
RUN npm install
RUN ng build

FROM nginx AS final
RUN apt update && apt install -y wget
ENV DOCKERIZE_VERSION v0.15.1
RUN wget https://github.com/powerman/dockerize/releases/download/$DOCKERIZE_VERSION/dockerize-linux-x86_64 \
    && mv dockerize-linux-x86_64 dockerize \
    && chmod +x dockerize \
    && mv dockerize /usr/local/bin
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=build /client/dist/abacuza-admin /usr/share/nginx/html
RUN mkdir /app-js-temp && cp /usr/share/nginx/html/*.js /app-js-temp
CMD dockerize -template /app-js-temp:/usr/share/nginx/html nginx -g "daemon off;"
