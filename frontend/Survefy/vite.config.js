import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'
import tsconfigPaths from "vite-tsconfig-paths"

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(),tailwindcss(),tsconfigPaths()],
  server: {
    proxy: {
      '^/pingauth': {
        target: 'https://localhost:44308',
        secure: false,
        changeOrigin: true,
      },
      '^/logout': {
        target: 'https://localhost:44308',
        secure: false,
        changeOrigin: true,
      },
      '^/login': {
        target: 'https://localhost:44308',
        secure: false,
        changeOrigin: true,
      },
      '^/getcuruserid': {
        target: 'https://localhost:44308',
        secure: false,
        changeOrigin: true,
      },
    },
  },
})
