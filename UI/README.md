# EzStock

## Run in container

1. In the `vite.config.ts` file, change the like `target: "http://127.0.0.1:7274/api"` to `target: "http://host.docker.internal:7274/api"`.
