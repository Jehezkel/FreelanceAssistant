/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,ts}",],
  theme: {
    extend: {
        boxShadow: {
        '5xl': '0 3px 10px rgba(0,0,0,0.19),0 1px 6px rgba(0,0,0,0.15)',
      }
    },
  },
  plugins: [],
}
