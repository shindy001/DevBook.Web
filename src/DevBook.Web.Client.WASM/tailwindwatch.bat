:: Check if tailwindcss is running -> if not, start it in watch mode for hot reload
TASKLIST | FINDSTR tailwindcss || tailwindcss -i .\Styles\tailwind.css -o .\wwwroot\tailwind.css --watch