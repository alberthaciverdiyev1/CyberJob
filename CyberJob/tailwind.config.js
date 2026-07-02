/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './Views/**/*.cshtml',
    './Pages/**/*.cshtml',
    './wwwroot/js/**/*.js',
  ],
  darkMode: 'class',
  theme: {
    extend: {
      animation: {
        'infinite-scroll': 'infinite-scroll 25s linear infinite',
      },
      keyframes: {
        'infinite-scroll': {
          from: {transform: 'translateX(0)'},
          to: {transform: 'translateX(-50%)'},
        }
      },
      colors: {
        cyberMain: '#0B2036',
        cyberDark: '#010C0F',
        cyberGreen: '#009689',
        cyberGray: '#262626',
        promoGreen: '#00FF00',
        darkGreen: '#154E4D',
        lightGreen: '#1A998D',
        darkMode: '#010C0F',
        extraLightGreen: '#00D4AA',
        gold: '#C29D55',
        goldStart: '#F3E2B9',
        goldEnd: '#B08B45',
        tableColor: '#293047',
        lightBlue: '#2D9CDB',
        extraLightBlue: '#9DD6FC',
      },
    },
  },
  plugins: [],
}
