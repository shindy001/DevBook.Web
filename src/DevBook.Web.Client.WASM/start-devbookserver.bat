:: Check if devbook-server is running -> if not, start it on port 7126, this script expects devbook-server in src as submodule
TASKLIST | FINDSTR DevBook.API || dotnet run --project ./../devbook-server/src/DevBook.API/DevBook.API.csproj --urls=https://localhost:7126/