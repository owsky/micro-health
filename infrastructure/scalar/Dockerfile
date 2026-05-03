FROM nginx:1.27.5-alpine

COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY index.html /usr/share/nginx/html/index.html.template

CMD ["/bin/sh", "-c", \
  "envsubst '${SCALAR_CLIENT_ID} ${SCALAR_CLIENT_SECRET} ${SCALAR_USERNAME} ${SCALAR_PASSWORD}' \
    < /usr/share/nginx/html/index.html.template \
    > /usr/share/nginx/html/index.html \
  && nginx -g 'daemon off;'"]
