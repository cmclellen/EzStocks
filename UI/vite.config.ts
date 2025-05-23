import { defineConfig } from "vite";
import tailwindcss from "@tailwindcss/vite";

// https://vite.dev/config/
export default defineConfig({
  plugins: [tailwindcss()],
  build: {
    target: "esnext",
  },
  server: {
    watch: {
      usePolling: true,
    },
    host: true,
    cors: true,
    proxy: {
      "/api": {
        target: "http://127.0.0.1:7274/api",
        secure: false,
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/api/, ""),
      },
    },
  },
});
