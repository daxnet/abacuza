FROM node:12.18.4-alpine AS build
RUN npm install -g @angular/cli
RUN mkdir /client
WORKDIR /client
COPY . .
RUN npm install
RUN ng build

FROM nginx AS final
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=build /client/dist/abacuza-admin /usr/share/nginx/html
