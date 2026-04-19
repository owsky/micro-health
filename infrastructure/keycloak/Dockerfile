FROM quay.io/keycloak/keycloak:26.6.0

# Build-time configuration — must match runtime environment
ENV KC_DB=postgres
ENV KC_HEALTH_ENABLED=true
ENV KC_HTTP_ENABLED=true
ENV KC_HOSTNAME_STRICT=false

# Pre-build Keycloak so runtime can use --optimized (much faster startup)
RUN /opt/keycloak/bin/kc.sh build

ENTRYPOINT ["/opt/keycloak/bin/kc.sh"]
