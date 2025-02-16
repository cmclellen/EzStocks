/** @type {import('tailwindcss').Config} */
export default {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  theme: {
    extend: {
      colors: {
        background: {
          DEFAULT: "rgba(var(--background))",
        },
        primary: {
          DEFAULT: "rgba(var(--primary))",
        },
        "on-primary": {
          DEFAULT: "rgba(var(--on-primary))",
        },
        secondary: {
          DEFAULT: "rgba(var(--secondary))",
        },
        "on-secondary": {
          DEFAULT: "rgba(var(--on-secondary))",
        },
        tertiary: {
          DEFAULT: "rgba(var(--tertiary))",
        },
        "on-tertiary": {
          DEFAULT: "rgba(var(--on-tertiary))",
        },
      },
    },
  },
  plugins: [],
  darkMode: "selector",
};
