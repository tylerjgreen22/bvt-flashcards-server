/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
    './features/**/*.{js,ts,jsx,tsx}',
  ],
  theme: {
    fontFamily: {
      'sans': ['"Plus Jakarta Sans"', 'sans-serif']
    },
    extend: {
      colors: {
        transparent: 'transparent',
        current: 'currentColor',
        'primary': '#4BBEFF',
        'secondary': '#F1FAFE',
        'accent': '#9C73F6',
        'text': '#040B02',
        'mobile': '#D9D9D9'
      }
    },
  },
  plugins: [],
}

