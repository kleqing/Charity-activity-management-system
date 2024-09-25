/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        './Pages/**/*.cshtml',
        './Views/**/*.cshtml',
        './Areas/**/*.cshtml',
    ],
    theme: {
        extend: {},
    },

    daisyui: {
        themes: [
            {
                mytheme: {
                    "primary": "#2563eb",
                    "primary-content": "#d2e2ff",
                    "secondary": "#facc15",
                    "secondary-content": "#150f00",
                    "accent": "#7c3aed",
                    "accent-content": "#e1dbff",
                    "neutral": "#fecdd3",
                    "neutral-content": "#160f10",
                    "base-100": "#F5F5F5",
                    "base-200": "#d3d4d6",
                    "base-300": "#b4b5b7",
                    "base-content": "#141415",
                    "info": "#3b82f6",
                    "info-content": "#010615",
                    "success": "#22c55e",
                    "success-content": "#000e03",
                    "warning": "#fb923c",
                    "warning-content": "#150801",
                    "error": "#ef4444",
                    "error-content": "#140202",
                },
            },
        ],
    },
    plugins: [
        require('daisyui')
    ],
}

