/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        './Pages/**/*.cshtml',
        './Views/**/*.cshtml'
    ],
    theme: {
        extend: {},
    },
    daisyui: {
        themes: ["light", "dark", "cupcake"],
    },
    plugins: [
        require('daisyui'),
    ],
}

