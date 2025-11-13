FROM nginx:alpine

COPY ClientNode/ /usr/share/nginx/html/

COPY nginx-client.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]

# docker build -t client-node .
# docker run --rm -p 3450:80 client-node