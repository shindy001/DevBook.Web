:: Check if tailwindcss.exe is running -> if not, start it in watch mode for hot reload
TASKLIST | FINDSTR tailwindcss.exe || tailwindcss.exe -i .\Styles\tailwind.css -o .\wwwroot\tailwind.css --watch