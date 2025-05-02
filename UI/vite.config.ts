import { defineConfig } from "vite";
import tailwindcss from "@tailwindcss/vite";
import eslint from "vite-plugin-eslint"; // eslint-ignore-line

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    tailwindcss(),
    eslint({
      include: ["src/**/*.ts", "src/**/*.tsx"],
    }),
  ],
  server: {
    watch: {
      usePolling: true,
    },
    host: true,
    cors: true,
    proxy: {
      "/api": {
        target: "http://host.docker.internal:7274/api",
        secure: false,
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/api/, ""),
      },
    },
  },
});
