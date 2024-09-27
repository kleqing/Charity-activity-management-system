/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        './Pages/**/*.cshtml',
        './Views/**/*.cshtml',
        './Areas/**/*.cshtml',
    ],
    safelist: [
        {
            pattern: /bg-+/, // 👈  This includes bg of all colors and shades
        },
    ],
    theme: {
        extend: {},
    },
    daisyui: {
        themes: [
            {
                mytheme: {
                    primary: "#1E429F",
                    // "primary-content": "#2F435A",
                    secondary: "#FACC15",
                    // "secondary-content": "#150f00",
                    accent: "#E2E7E9",
                    "accent-content": "#150f00",
                    neutral: "#FEC9C9",
                    "neutral-content": "#000000",
                    "base-100": "#F5F5F5",
                    "base-200": "#D3D4D6",
                    "base-300": "#B4B5B7",
                    // "base-content": "#150f00",
                    info: "#3a5d9c",
                    "info-content": "#f5f5f5",
                    success: "#22c55e",
                    "success-content": "#f5f5f5",
                    warning: "#FF8225",
                    "warning-content": "#f5f5f5",
                    error: "#ED4040",
                    "error-content": "#f5f5f5",
                },
            },
        ],
    },
    plugins: [
        require('daisyui'),
    ],
}

